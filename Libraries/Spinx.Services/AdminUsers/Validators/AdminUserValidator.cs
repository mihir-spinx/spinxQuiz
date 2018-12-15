using FluentValidation;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;
using System.Linq;

namespace Spinx.Services.AdminUsers.Validators
{
    public class AdminUserValidator : AbstractValidator<AdminUserDto>
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public AdminUserValidator(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;

            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Email).NotEmpty().EmailAddress().MaximumLength(100)
                .Must(UniqueEmail).WithMessage("{PropertyName} already used with other user.");

            When(v => v.Id == 0 || !string.IsNullOrEmpty(v.Password), () =>
            {
                RuleFor(v => v.Password).NotEmpty().Length(6, 20);
            });

            RuleFor(v => v.Roles).NotNull();
        }

        private bool UniqueEmail(AdminUserDto dto, string email)
        {
            var query = _adminUserRepository.AsNoTracking.Where(w => w.Email == email);

            if (dto.Id == 0) return !query.Any();

            return !query.Any(w => w.Id != dto.Id);
        }
    }
}