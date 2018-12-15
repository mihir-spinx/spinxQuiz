using Spinx.Core;
using Spinx.Core.Helpers;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Domain.AdminUsers;
using Spinx.Services.AdminAuth.DTOs;
using Spinx.Services.AdminAuth.Validators;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.AdminAuth
{
    public interface IAdminForgotPasswordService
    {
        Result ForgotPassword(AdminForgotPasswordDto dto, out AdminUser adminUser);
    }

    public class AdminForgotPasswordService : IAdminForgotPasswordService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminForgotPasswordService(
            IAdminUserRepository adminUserRepository,
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Result ForgotPassword(AdminForgotPasswordDto dto, out AdminUser adminUser)
        {
            adminUser = null;

            var validator = new AdminForgotPasswordValidator();
            var result = validator.ValidateResult(dto);
            if (!result.Success) return result;

            result = ForgotPasswordResponse(dto.Email);

            adminUser = _adminUserRepository.AsNoTracking
                .FirstOrDefault(w => w.IsActive && w.Email == dto.Email);

            if (adminUser != null)
            {
                adminUser = new AdminUser()
                {
                    Name = adminUser.Name,
                    Email = adminUser.Email,
                    ForgotPasswordToken = GenerateAndSaveForgotPasswordToken(adminUser)
                };
            }

            return result;
        }

        private string GenerateAndSaveForgotPasswordToken(AdminUser adminUser)
        {
            var passwordResetToken = StringHelper.RandomString(12);

            adminUser.ForgotPasswordToken = passwordResetToken;
            _adminUserRepository.Update(adminUser);
            _unitOfWork.Commit();

            return passwordResetToken;
        }

        private static Result ForgotPasswordResponse(string email)
        {
            return new Result()
            {
                Success = true,
                MessageType = MessageType.Success,
                Message = $"If there is an account associated with <b>{email}</b> you will receive an email with a link to reset your password."
            };
        }
    }
}