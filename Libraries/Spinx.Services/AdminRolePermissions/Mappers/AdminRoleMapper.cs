using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Spinx.Domain.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.DTOs;
using System.Linq;

namespace Spinx.Services.AdminRolePermissions.Mappers
{
    public static class AdminRoleMapper
    {
        public static void Init()
        {
            Mapper.AddMap<AdminRole, AdminRoleDto>(src =>
            {
                var adminUserViewModel = new AdminRoleDto();
                adminUserViewModel.InjectFrom(src);
                adminUserViewModel.Permissions = src.Permissionses.Select(s => s.Id).ToList();

                return adminUserViewModel;
            });

            Mapper.AddMap<AdminRoleDto, AdminRole>((from, to) =>
            {
                var existing = to as AdminRole ?? new AdminRole();
                existing.InjectFrom(new LoopInjection(new[] { "Permissions" }), from);
                return existing;
            });
        }
    }
}