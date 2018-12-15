using FluentValidation;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;
using System.Linq;

namespace Spinx.Services.AdminUsers.Validators
{
    public class EditProfileValidator : AbstractValidator<EditProfileDto>
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public EditProfileValidator(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;

            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Email).NotEmpty().EmailAddress().MaximumLength(100)
                .Must(UniqueEmail).WithMessage("{PropertyName} already used with other user.");
        }

        private bool UniqueEmail(EditProfileDto dto, string email)
        {
            return !_adminUserRepository.AsNoTracking.Any(w => w.Id != dto.Id && w.Email == email);
        }
    }
}