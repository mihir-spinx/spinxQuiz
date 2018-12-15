using FluentValidation;
using Spinx.Services.AdminAuth.DTOs;

namespace Spinx.Services.AdminAuth.Validators
{
    public class AdminLoginValidator : AbstractValidator<AdminLoginDto>
    {
        public AdminLoginValidator()
        {
            RuleFor(v => v.Email).NotEmpty().EmailAddress();
            RuleFor(v => v.Password).NotEmpty();
        }
    }
}