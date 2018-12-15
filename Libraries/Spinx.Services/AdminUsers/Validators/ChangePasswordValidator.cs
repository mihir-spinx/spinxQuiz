using FluentValidation;
using Spinx.Core.Encryption;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;

namespace Spinx.Services.AdminUsers.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public ChangePasswordValidator(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;

            RuleFor(v => v.CurrentPassword).NotEmpty().Must(ValidateCurrentPassword).WithMessage("Invalid current password.");
            RuleFor(v => v.NewPassword).NotEmpty().Length(6, 20)
                .Equal(v => v.ConfirmPassword).WithMessage("'New Password' should match to 'Confirm Password'.");
            RuleFor(v => v.ConfirmPassword).NotEmpty();
        }

        private bool ValidateCurrentPassword(ChangePasswordDto dto, string password)
        {
            var adminUser = _adminUserRepository.Find(dto.Id);
            return SecurityHelper.VerifyHash(dto.CurrentPassword, adminUser.Password, adminUser.Salt);
        }
    }
}