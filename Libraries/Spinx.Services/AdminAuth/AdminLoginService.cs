using Spinx.Core;
using Spinx.Core.Encryption;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Domain.AdminUsers;
using Spinx.Services.AdminAuth.DTOs;
using Spinx.Services.AdminAuth.Validators;
using Spinx.Services.Infrastructure;
using System;
using System.Linq;

namespace Spinx.Services.AdminAuth
{
    public interface IAdminLoginService
    {
        Result Login(AdminLoginDto dto, out AdminUser adminUser);
    }

    public class AdminLoginService : IAdminLoginService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminLoginService(
            IAdminUserRepository adminUserRepository,
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Result Login(AdminLoginDto dto, out AdminUser adminUser)
        {
            adminUser = null;

            var validator = new AdminLoginValidator();
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            adminUser = _adminUserRepository.AsNoTracking
                .FirstOrDefault(w => w.IsActive && w.Email == dto.Email);

            return adminUser == null ?
                SendFailedLoginResponse() :
                VerifyPassword(dto.Password, adminUser);
        }

        private Result VerifyPassword(string password, AdminUser adminUser)
        {
            if (!SecurityHelper.VerifyHash(password, adminUser.Password, adminUser.Salt))
                return SendFailedLoginResponse();

            SetLastLoginAt(adminUser);

            return new Result().SetSuccess();
        }

        private static Result SendFailedLoginResponse()
        {
            return new Result()
                .SetError("You did not sign in correctly or your account is temporarily disabled.");
        }

        private void SetLastLoginAt(AdminUser adminUser)
        {
            adminUser.LastLoginAt = DateTime.Now;

            _adminUserRepository.Update(adminUser);
            _unitOfWork.Commit();
        }
    }
}