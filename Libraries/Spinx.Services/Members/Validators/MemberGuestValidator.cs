using FluentValidation;
using Spinx.Services.Members.DTOs;

namespace Spinx.Services.Members.Validators
{
    public class MemberGuestValidator : AbstractValidator<MemberGuestDto>
    {
        public MemberGuestValidator()
        {
            RuleFor(v => v.Password).NotEmpty().MaximumLength(20);
            RuleFor(v => v.IAgree).NotEmpty().WithMessage("You must agree with the terms and conditions.");
        }
    }
}