using FluentValidation;
using Spinx.Services.AdminAuth.DTOs;

namespace Spinx.Services.AdminAuth.Validators
{
    public class AdminResetPasswordValidator : AbstractValidator<AdminResetPasswordDto>
    {
        public AdminResetPasswordValidator()
        {
            RuleFor(v => v.Email).NotEmpty().EmailAddress();
            RuleFor(v => v.Password).NotEmpty().Length(6, 20)
                .Equal(v => v.ConfirmPassword).WithMessage("'Password' should match to 'Confirm Password'.");
            RuleFor(v => v.ConfirmPassword).NotEmpty().Length(6, 20);
        }
    }
}