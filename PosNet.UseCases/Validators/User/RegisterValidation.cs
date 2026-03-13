

using FluentValidation;
using PosNet.UseCases.Dtos.Auth;

namespace PosNet.UseCases.Validators.User;

public class RegisterValidation : AbstractValidator<RegisterDto>
{
    public RegisterValidation()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("El email es obligatorio")
        .EmailAddress().WithMessage("El email es invalido");
        RuleFor(x => x.Username).NotEmpty().WithMessage("El nombre de usuario es obligatorio")
        .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres");
        RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es obligatoria")
        .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
        RuleFor(x => x.RoleId).NotEmpty().WithMessage("El rol es obligatorio")
        .GreaterThan(0).WithMessage("El identificador del rol debe ser invalido");
    }
}