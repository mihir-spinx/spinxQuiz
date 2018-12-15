using FluentValidation;
using Spinx.Services.Members.DTOs;

namespace Spinx.Services.Members.Validators
{
    public class MemberForgotPasswordValidator : AbstractValidator<MemberForgotPasswordDto>
    {
        public MemberForgotPasswordValidator()
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Invalid email address.").EmailAddress().WithMessage("Invalid email address.");
        }
    }
}