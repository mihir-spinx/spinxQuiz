using Spinx.Core;
using Spinx.Core.Encryption;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Member;
using Spinx.Domain.Members;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members.DTOs;
using Spinx.Services.Members.Validators;
using System.Linq;

namespace Spinx.Services.Members
{
    public interface IMemberResetPasswordService
    {
        bool IsValidToken(string token);

        Result ResetPassword(MemberChangePasswordDto dto);
    }

    public class MemberResetPasswordService : IMemberResetPasswordService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MemberResetPasswordService(
                    IMemberRepository memberRepository, 
                    IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public bool IsValidToken(string token)
        {
            return !string.IsNullOrEmpty(token) &&
                   _memberRepository.AsNoTracking.Any(w => w.ForgotPasswordToken == token && w.IsActive);
        }

        public Result ResetPassword(MemberChangePasswordDto dto)
        {
            var validator = new MemberChangePasswordValidator();
            var result = validator.ValidateResult(dto);

            if (!result.Success) return result;

            var member =_memberRepository.AsNoTracking.FirstOrDefault(w => w.ForgotPasswordToken == dto.Token && w.IsActive);

            if (member == null)
                return new Result().SetError("Invalid forgot password token or token already expired.").SetBlankRedirect();

            SetNewPassword(member, dto.NewPassword);

            return new Result().SetBlankRedirect()
                .SetSuccess("Your password has been updated successfully. Please login with your new password.");
        }

        private void SetNewPassword(Member member, string newPassword)
        {
            member.Salt = SecurityHelper.GenerateSalt();
            member.Password = SecurityHelper.GenerateHash(newPassword, member.Salt);
            member.ForgotPasswordToken = null;

            _memberRepository.Update(member);

            _unitOfWork.Commit();
        }
    }
}