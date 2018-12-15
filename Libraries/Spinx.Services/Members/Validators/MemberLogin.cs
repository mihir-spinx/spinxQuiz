using FluentValidation;
using Spinx.Services.Members.DTOs;

namespace Spinx.Services.Members.Validators
{
    public class MemberLoginValidator : AbstractValidator<MemberFrontDto>
    {
        public MemberLoginValidator()
        {
            RuleFor(v => v.Password).NotEmpty();
            RuleFor(v => v.Email).NotEmpty().WithMessage("Invalid email address.").EmailAddress().WithMessage("Invalid email address.");
        }
    }
}