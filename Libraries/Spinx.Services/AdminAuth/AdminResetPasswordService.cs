using Spinx.Core;
using Spinx.Core.Encryption;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Domain.AdminUsers;
using Spinx.Services.AdminAuth.DTOs;
using Spinx.Services.AdminAuth.Validators;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.AdminAuth
{
    public interface IAdminResetPasswordService
    {
        bool IsValidToken(string token);

        Result ResetPassword(AdminResetPasswordDto dto);
    }

    public class AdminResetPasswordService : IAdminResetPasswordService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminResetPasswordService(
            IAdminUserRepository adminUserRepository, 
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;
        }

        public bool IsValidToken(string token)
        {
            return !string.IsNullOrEmpty(token) &&
                   _adminUserRepository.AsNoTracking.Any(w => w.ForgotPasswordToken == token && w.IsActive);
        }

        public Result ResetPassword(AdminResetPasswordDto dto)
        {
            var validator = new AdminResetPasswordValidator();
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            var adminUser =
                _adminUserRepository.AsNoTracking.FirstOrDefault(w =>
                    w.Email == dto.Email && w.ForgotPasswordToken == dto.Token && w.IsActive);

            if (adminUser == null)
                return new Result().SetError("Invalid forgot password token or token already expired.").SetBlankRedirect();

            SetNewPassword(adminUser, dto.Password);

            return new Result().SetBlankRedirect()
                .SetSuccess("Your password has been updated successfully. Please login with your new password.");
        }

        private void SetNewPassword(AdminUser adminUser, string newPassword)
        {
            adminUser.Salt = SecurityHelper.GenerateSalt();
            adminUser.Password = SecurityHelper.GenerateHash(newPassword, adminUser.Salt);
            adminUser.ForgotPasswordToken = null;
            _adminUserRepository.Update(adminUser);
            _unitOfWork.Commit();
        }
    }
}