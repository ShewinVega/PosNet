using System.ComponentModel.DataAnnotations;

namespace PosNet.UseCases.Dtos.Roles
{
    public class RoleDto : BaseDto<RoleDto, Role>
    {
        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public int Id { get; set; }

        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public string Name { get; set; }
    }
}
