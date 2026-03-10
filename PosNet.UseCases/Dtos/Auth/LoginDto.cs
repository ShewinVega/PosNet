using Mapster;
using System.ComponentModel.DataAnnotations;

namespace PosNet.UseCases.Dtos.Auth
{
    public class LoginDto : BaseDto<LoginDto, User>
    {
        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public string Identifier { get; set; }

        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public string Password { get; set; }

        public override void AddCustomMappings(TypeAdapterConfig config)
        {
            SetCustomMappings(config)
                .Map(dest => dest.PasswordHash, src => src.Password);
        }
    }
}
