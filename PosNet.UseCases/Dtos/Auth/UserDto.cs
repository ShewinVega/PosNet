
using PosNet.UseCases.Dtos.Roles;

namespace PosNet.UseCases.Dtos.Auth
{
    public class UserDto : BaseDto<UserDto, User>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public RoleDto Role { get; set; }
    }
}
