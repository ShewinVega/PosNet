using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PosNet.Constants;
using PosNet.DTOs;
using PosNet.Models;
using PosNet.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PosNet.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IValidator<AuthDto> registerValidator) : ControllerBase
    {


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(AuthDto request) 
        {
            // Request data entry validation
            var validationResult = await registerValidator.ValidateAsync(request);

            if(!validationResult.IsValid)
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

            if(newUser == null)
            {
                return BadRequest("Username already exists");
            }

            return Ok(newUser);
        }


        [HttpPost("login")]
        public ActionResult<TokenResponseDto> Login(AuthDto request)
        {
            // User exist
            var user = authService.GetUser(request.Username).Result;

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var loginResult = authService.Login(request, user).Result;

            if(loginResult == null)
            {
                return BadRequest("User or password credentials are not correct!!!");
            }

            return Ok(loginResult);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
        {
            var response = await authService.RefreshTokenAsync(request);
            if(response is null || response.AccessToken is null || response.RefreshToken is null)
            {
                return Unauthorized("Invalid token");
            }
            return Ok(response);
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
            return Ok("You are  an Admin");
        }

    }
}
