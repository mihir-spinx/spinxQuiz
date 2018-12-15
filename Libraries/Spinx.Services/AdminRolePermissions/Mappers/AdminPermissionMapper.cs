using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Spinx.Domain.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.DTOs;

namespace Spinx.Services.AdminRolePermissions.Mappers
{
    public static class AdminPermissionMapper
    {
        public static void Init()
        {
            Mapper.AddMap<AdminPermission, AdminPermissionDto>(src =>
            {
                var adminUserViewModel = new AdminPermissionDto();
                adminUserViewModel.InjectFrom(src);

                if (src.ParentId > 0)
                    adminUserViewModel.IsParentSelected = true;

                return adminUserViewModel;
            });

            Mapper.AddMap<AdminPermissionDto, AdminPermission>((from, to) =>
            {
                var existing = to as AdminPermission ?? new AdminPermission();
                existing.InjectFrom(new LoopInjection(new[] { "ParentId", "Left", "Right" }), from);

                if (from.IsParentSelected)
                    existing.ParentId = from.ParentId;

                return existing;
            });
        }
    }
}