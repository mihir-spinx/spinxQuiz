using FluentValidation;
using Spinx.Core.Encryption;
using Spinx.Data.Repository.Member;
using Spinx.Services.Members.DTOs;

namespace Spinx.Services.Members.Validators
{
    public class MemberFrontChangePasswordValidator : AbstractValidator<MemberFrontChangePasswordDto>
    {
        private readonly IMemberRepository _memberRepository;
        public MemberFrontChangePasswordValidator(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
            RuleFor(v => v.CurrentPassword).NotEmpty().Must(ValidateCurrentPassword).WithMessage("Invalid current password.");
            RuleFor(v => v.NewPassword).NotEmpty().Length(6, 20)
                .Equal(v => v.ConfirmPassword).WithMessage("'New Password' should match to 'Confirm Password'.");
            RuleFor(v => v.ConfirmPassword).NotEmpty().Length(6, 20);
        }

        private bool ValidateCurrentPassword(MemberFrontChangePasswordDto dto, string password)
        {
            var member = _memberRepository.Find(dto.Id);
            return SecurityHelper.VerifyHash(dto.CurrentPassword, member.Password, member.Salt);
        }
    }
}