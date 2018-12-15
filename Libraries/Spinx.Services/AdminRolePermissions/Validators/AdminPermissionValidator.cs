using FluentValidation;
using Spinx.Data.Repository.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.DTOs;
using System.Linq;

namespace Spinx.Services.AdminRolePermissions.Validators
{
    public class AdminPermissionValidator : AbstractValidator<AdminPermissionDto>
    {
        private readonly IAdminPermissionRepository _adminPermissionRepository;

        public AdminPermissionValidator(IAdminPermissionRepository adminPermissionRepository)
        {
            _adminPermissionRepository = adminPermissionRepository;

            RuleFor(v => v.IsParentSelected).NotEmpty().WithName("Parent Selection");

            When(v => v.IsParentSelected, () =>
            {
                RuleFor(v => v.ParentId).NotEmpty().WithName("Parent Permission").NotEqual(v => v.Id)
                    .Must(ValidParent).WithMessage("Invalid parent permission selected.");
            });

            RuleFor(v => v.Name).NotEmpty().MaximumLength(100).Must(UniqueName)
                .WithMessage("System name already used with other permission.");
            
            RuleFor(v => v.DisplayName).NotEmpty().MaximumLength(100);
        }

        private bool UniqueName(AdminPermissionDto dto, string name)
        {
            var query = _adminPermissionRepository.AsNoTracking.Where(w => w.Name == name);

            if (dto.Id == 0)
                return !query.Any();

            return !query.Any(w => w.Id != dto.Id);
        }

        private bool ValidParent(AdminPermissionDto dto, int? id)
        {
            if (dto.Id == 0) return true;

            var childPermissionIds = _adminPermissionRepository.AsNoTracking
                .Where(w => w.Left >= dto.Left && w.Right <= dto.Right).Select(s => s.Id).ToList();

            return childPermissionIds.IndexOf(dto.ParentId ?? 0) == -1;
        }
    }
}