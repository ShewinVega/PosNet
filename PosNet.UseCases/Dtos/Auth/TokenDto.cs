using Mapster;
using System.ComponentModel.DataAnnotations;

namespace PosNet.UseCases.Dtos.Auth
{
    public class TokenDto : RefreshTokenDto
    {
        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public string AccessToken { get; set; }

    }
}
