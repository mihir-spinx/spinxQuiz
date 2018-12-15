using FluentValidation;
using Spinx.Services.Members.DTOs;

namespace Spinx.Services.Members.Validators
{
    public class MemberChangePasswordValidator : AbstractValidator<MemberChangePasswordDto>
    {
        public MemberChangePasswordValidator()
        {
            RuleFor(v => v.Token).NotEmpty();
            RuleFor(v => v.NewPassword).NotEmpty().Length(6, 20)
                .Equal(v => v.ConfirmPassword).WithMessage("'New Password' should match to 'Confirm Password'.");

            RuleFor(v => v.ConfirmPassword).NotEmpty().Length(6, 20);
        }
    }
}