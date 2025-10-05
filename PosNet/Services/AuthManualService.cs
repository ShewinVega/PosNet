using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PosNet.DTOs;
using PosNet.Models;
using PosNet.Repository.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PosNet.Services
{
    public class AuthManualService : IAuthManualService
    {

        private readonly IAuthManualRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManualService> _logger;

        public AuthManualService(
            IAuthManualRepository authRepository, 
            IMapper mapper, IConfiguration configuration,
            ILogger<AuthManualService> logger
            )
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<User?> Register(AuthDto request)
        {
            try
            {
                // Validate if user exists
                var userExists = await _authRepository.UsernameExists(request.Username);

                if(userExists)
                {
                    return null;
                }

                // Mapping
                var newUser = _mapper.Map<User>(request);

                // Hashing Password
                var hashedPassword = new PasswordHasher<User>()
                .HashPassword(newUser, request.Password);

                newUser.Username = request.Username;
                newUser.PasswordHash = hashedPassword;

                // Create User
                await _authRepository.Register(newUser);

                // Save to DB
                await _authRepository.Save();

                // Return new User
                return newUser;

            } catch (Exception error)
            {
                _logger.LogError($"There was an error in Auth services: {error}");
                throw new Exception(error.Message);
            }
        }

        public async Task<TokenResponseDto?> Login(AuthDto request, User user)
        {
            try
            {
                // Verify password
                if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
                {
                    return null;
                }

                // Create the token and refreshToken
                TokenResponseDto response = await CreateTokenResponse(user);

                return response;

            }
            catch (Exception error)
            {
                _logger.LogError($"There was an error in Auth services: {error}");
                throw new Exception(error.Message);
            }
        }

        public async Task<User?> GetUser(string username)
        {
            try
            {
                var user = await _authRepository.GetUser(username);

                if(user == null)
                {
                    return null;
                }

                return user;

            } catch(Exception error)
            {
                _logger.LogError($"There was an error in Auth services: {error}");
                throw new Exception(error.Message);
            }
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            // get the token
            var refreshToken = GenerateRefreshToken();

            // update the user with the new refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            // save the changes
            await _authRepository.Save();

            // return the new refreshToken
            return refreshToken;
        }


        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            try
            {
                // validate the token
                var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
                if (user is null)
                {
                    return null;
                }


                TokenResponseDto response = await CreateTokenResponse(user);
                return response;

            } catch(Exception error)
            {
                _logger.LogError($"There was an error in Auth services: {error}");
                throw new Exception(error.Message);
            }
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            try
            {
                // get the user by id
                var user = await _authRepository.GetUserById(userId);

                if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) 
                {
                    return null;
                }

                return user;
            }
            catch (Exception error)
            {
                _logger.LogError($"There was an error in Auth services: {error}");
                throw new Exception(error.Message);
            }
        }

        // Create the refresh token
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // Generate the token
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:secretKey")));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}