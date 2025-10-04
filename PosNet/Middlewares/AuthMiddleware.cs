using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PosNet.Middlewares
{
    public class AuthMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<AuthMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Variables
                var registerPath = context.Request.Path.Equals("/api/Auth/register", StringComparison.OrdinalIgnoreCase);
                var loginPath = context.Request.Path.Equals("/api/Auth/login", StringComparison.OrdinalIgnoreCase);
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var secretKey = _configuration.GetValue<string>("Jwt:secretKey");
                var validIssuer = _configuration.GetValue<string>("Jwt:Issuer");
                var validAudience = _configuration.GetValue<string>("Jwt:Audience");

                // Allow register and login paths without token
                if (registerPath || loginPath)
                {
                    await _next(context);
                    return;
                }

                if (!string.IsNullOrEmpty(token))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(secretKey);

                    

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = validIssuer,
                        ValidateAudience = true,
                        ValidAudience = validAudience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                    }, out SecurityToken validatedToken);

                    // If token is valid
                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                    context.Items["userId"] = userId;
                }

                if (string.IsNullOrEmpty(token))
                {
                    // theres always have to be a token
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token is missing");
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error in the Auth Middleware: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
                await context.Response.WriteAsync("Invalid Token");
                return;
            }

        }
    }
}
