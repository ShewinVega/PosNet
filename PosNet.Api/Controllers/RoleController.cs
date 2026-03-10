using Microsoft.AspNetCore.Mvc;
using PosNet.UseCases.Interfaces;

namespace PosNet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(
     IRoleService roleServices
     ) : ControllerBase
    {
        private readonly IRoleService _roleService = roleServices;

        [HttpGet("all")]
        public async Task<IActionResult> AllRoles()
        {
            var result = await _roleService.All();

            return Ok(result);
        }
    }
}
