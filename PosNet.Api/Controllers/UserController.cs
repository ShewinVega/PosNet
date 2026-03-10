using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PosNet.UseCases.Dtos.Pagination;
using PosNet.UseCases.Interfaces;

namespace PosNet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;


        [HttpGet("all")]
        public async Task<IActionResult> All([FromBody] PaginationDto request)
        {
            var result = await _userService.GetUsersPaginated(request);

            return Ok(result);
        }
    }
}
