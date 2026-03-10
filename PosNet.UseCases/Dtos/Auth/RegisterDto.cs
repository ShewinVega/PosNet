using Mapster;
using System.ComponentModel.DataAnnotations;

namespace PosNet.UseCases.Dtos.Auth
{
    public class RegisterDto : BaseDto<RegisterDto, User>
    {
        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public string Username { get; set; }

        [Required(ErrorMessage = "FIELD_REQUIRED")]
        [EmailAddress(ErrorMessage = "INVALID_EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public string Password { get; set; }

        [Required(ErrorMessage = "FIELD_REQUIRED")]
        [Range(0, int.MaxValue, ErrorMessage = "NEGATIVE_FIELD")]
        public int RoleId { get; set; }

        public override void AddCustomMappings(TypeAdapterConfig config)
        {
            SetCustomMappings(config)
                .Map(dest => dest.PasswordHash, src => src.Password);
        }
    }
}
