using Microsoft.AspNetCore.Mvc;
using PosNet.UseCases.Dtos.Auth;
using PosNet.UseCases.Interfaces;

namespace PosNet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IAuthService authService,
        IHandleBusinessError handleError
     ) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IHandleBusinessError _handleError = handleError;

        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _authService.Register(request);

            if(result.Success == false)
            {
                return StatusCode(_handleError.GetStatusCode(), _handleError.CreateProblemDetails());
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }


        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _authService.Login(request);

            if(result.Success == false)
            {
                return StatusCode(_handleError.GetStatusCode(), _handleError.CreateProblemDetails());
            }

            return Ok(result);
        }

        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("refresh_token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto request)
        {
            var result = await _authService.RefreshAccessTokenAsync(request);

            if( result.Success == false)
            {
                return StatusCode(_handleError.GetStatusCode(), _handleError.CreateProblemDetails());
            }

            return Ok(result);
        }
    }
}
