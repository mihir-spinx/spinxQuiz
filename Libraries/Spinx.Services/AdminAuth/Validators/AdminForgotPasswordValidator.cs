using FluentValidation;
using Spinx.Services.AdminAuth.DTOs;

namespace Spinx.Services.AdminAuth.Validators
{
    public class AdminForgotPasswordValidator : AbstractValidator<AdminForgotPasswordDto>
    {
        public AdminForgotPasswordValidator()
        {
            RuleFor(v => v.Email).NotEmpty().EmailAddress();
        }
    }
}