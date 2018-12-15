using FluentValidation;
using Spinx.Services.AdminRolePermissions.DTOs;

namespace Spinx.Services.AdminRolePermissions.Validators
{
    public class AdminRoleValidator : AbstractValidator<AdminRoleDto>
    {
        public AdminRoleValidator()
        {
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Permissions).NotNull();
        }
    }
}