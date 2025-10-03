using FluentValidation;
using PosNet.DTOs;

namespace PosNet.Validators.User
{
    public class RegisterValidation:AbstractValidator<AuthDto>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
                .MaximumLength(20).WithMessage("Username must be at most 20 characters long");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        }
    }
}
