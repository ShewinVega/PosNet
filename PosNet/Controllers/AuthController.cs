using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosNet.Constants;
using PosNet.DTOs;
using PosNet.Models;
using PosNet.Services;

namespace PosNet.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IValidator<AuthDto> registerValidator, ILogger<AuthController> logger) : ControllerBase
    {


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(AuthDto request) 
        {
            try
            {
                // Request data entry validation
                var validationResult = await registerValidator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // User exist
                var user = authService.GetUser(request.Username).Result;

                if (user != null)
                {
                    return BadRequest("Username already exists");
                }


                var newUser = await authService.Register(request);

                if (newUser == null)
                {
                    return BadRequest("Username already exists");
                }

                return Ok(newUser);
            } catch(Exception error)
            {
                logger.LogError($"There was an error in Auth controller: {error}");
                return StatusCode(500, "An unexpected error has ocurred, please try later");
            }
        }


        [HttpPost("login")]
        public ActionResult<TokenResponseDto> Login(AuthDto request)
        {
            try
            {
                // User exist
                var user = authService.GetUser(request.Username).Result;

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                var loginResult = authService.Login(request, user).Result;

                if (loginResult == null)
                {
                    return BadRequest("User or password credentials are not correct!!!");
                }

                return Ok(loginResult);
            } catch (Exception error)
            {
                logger.LogError($"There was an error in Auth controller: {error}");
                return StatusCode(500, "An unexpected error has ocurred, please try later");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
        {
            try
            {
                var response = await authService.RefreshTokenAsync(request);
                if (response is null || response.AccessToken is null || response.RefreshToken is null)
                {
                    return Unauthorized("Invalid token");
                }
                return Ok(response);
            } catch (Exception error)
            {
                logger.LogError($"There was an error in Auth controller: {error}");
                return StatusCode(500, "An unexpected error has ocurred, please try later");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!!!");
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("admin")]
        public IActionResult AdminOnlyEndpoint()
        {
            // get the userId
            var userId = HttpContext.Items.TryGetValue("userId", out var id) ? id : null;
            Console.WriteLine($"ESTE ES EL ID DEL USUARIO: {userId}");

            return Ok("You are  an Admin");
        }

    }
}
