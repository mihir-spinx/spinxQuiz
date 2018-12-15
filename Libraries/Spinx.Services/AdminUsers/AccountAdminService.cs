using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Core.Encryption;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Domain.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Services.AdminUsers.Validators;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.AdminUsers
{
    public interface IAccountAdminService
    {
        EditProfileDto GetEditProfile(int userId);
        Result SaveEditProfile(EditProfileDto model);

        Result ChangePassword(ChangePasswordDto model);
    }

    public class AccountAdminService : IAccountAdminService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EditProfileValidator _editProfileValidator;
        private readonly ChangePasswordValidator _changePasswordValidator;

        public AccountAdminService(
            IAdminUserRepository adminUserRepository,
            IUnitOfWork unitOfWork,
            EditProfileValidator editProfileValidator,
            ChangePasswordValidator changePasswordValidator)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;
            _editProfileValidator = editProfileValidator;
            _changePasswordValidator = changePasswordValidator;
        }

        #region Edit Profile

        public EditProfileDto GetEditProfile(int userId)
        {
            var adminUser = _adminUserRepository.Find(userId);

            return adminUser == null ?
                null :
                Mapper.Map<EditProfileDto>(adminUser);
        }

        public Result SaveEditProfile(EditProfileDto dto)
        {
            var result = _editProfileValidator.ValidateResult(dto);
            if (!result.Success) return result;

            var dbAdminUser = _adminUserRepository.Find(dto.Id);

            if (dbAdminUser == null) return null;

            EditProfileSave(dto, dbAdminUser);

            return new Result().SetSuccess(Messages.ProfileUpdated);
        }

        private void EditProfileSave(EditProfileDto model, AdminUser adminUser)
        {
            adminUser.Name = model.Name;
            adminUser.Email = model.Email;
            _adminUserRepository.Update(adminUser);
            _unitOfWork.Commit();
        }

        #endregion

        #region Change Password

        public Result ChangePassword(ChangePasswordDto dto)
        {
            var result = _changePasswordValidator.ValidateResult(dto);

            if (!result.Success)
                return result;

            var adminUser = _adminUserRepository.Find(dto.Id);

            ChangePasswordSave(dto.NewPassword, adminUser);

            return new Result().SetSuccess(Messages.PasswordUpdated).Clear();
        }

        private void ChangePasswordSave(string newPassword, AdminUser adminUser)
        {
            var salt = SecurityHelper.GenerateSalt();
            var encryptedPassword = SecurityHelper.GenerateHash(newPassword, salt);
            adminUser.Salt = salt;
            adminUser.Password = encryptedPassword;
            _adminUserRepository.Update(adminUser);
            _unitOfWork.Commit();
        }

        #endregion      
    }
}