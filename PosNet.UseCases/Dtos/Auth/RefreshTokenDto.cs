using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosNet.UseCases.Dtos.Auth
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "FIELD_REQUIRED")]
        public string RefreshToken { get; set; }
    }
}
