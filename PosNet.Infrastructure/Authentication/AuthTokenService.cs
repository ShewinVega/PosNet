
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PosNet.Domain.Interfaces;
using PosNet.Domain.Shared;
using PosNet.Infrastructure.Security;
using PosNet.UseCases.Dtos.Auth;
using PosNet.UseCases.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PosNet.Infrastructure.Authentication
{
    public class AuthTokenService(
     IUnitOfWork unitOfWork,
     IOptions<JwtSettings> jwtOptions,
     ILogger<AuthTokenService> logger
     ) : ITokenAuthService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly JwtSettings _jwtSettings = jwtOptions.Value;
        private readonly ILogger<AuthTokenService> _logger = logger;
        public async Task<Result<TokenDto>> CreateTokenResponse(User user)
        {
            var refreshTokenResult = await GenerateAndSaveRefreshTokenAsync(user);
            var result =  new TokenDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = refreshTokenResult.RefreshToken,
            };

            return Result<TokenDto>.Ok( result );
        }

        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: GenerateClaims(user),
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private List<Claim> GenerateClaims(User user)
        {
            List<Claim> claims = [
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Name, user.Username)
            ];

            // Role
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));

            // Add permissions
            var permissions = user.Role.RolesPermissions?.Select(rp => rp.Permission?.Name)
                .Where(name => name != null)
                .ToList() ?? new List<string>();


            if (permissions.Count > 0)
            {
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("Permission", permission));
                }

                _logger.LogInformation("Permissions added successfully");
            }

            return claims;
        }

        private async Task<RefreshTokenDto> GenerateAndSaveRefreshTokenAsync(User user)
        {
            // Get Token
            var refreshToken = GenerateRefreshToken();

            // Update user with the new refreshToken
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            // Save Changes
            await _unitOfWork.Save();

            return new RefreshTokenDto
            {
                RefreshToken = refreshToken,
            };
        }

        private string GenerateRefreshToken()
        {
            var ramdonNumber = new Byte[32];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(ramdonNumber);
                return Convert.ToBase64String(ramdonNumber);
            }

        }
    }
}
