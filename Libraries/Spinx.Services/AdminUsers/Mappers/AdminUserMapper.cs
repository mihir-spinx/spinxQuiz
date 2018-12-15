using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Spinx.Domain.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;
using System.Linq;

namespace Spinx.Services.AdminUsers.Mappers
{
    public static class AdminUserMapper
    {
        public static void Init()
        {
            Mapper.AddMap<AdminUser, AdminUserDto>(src =>
            {
                var adminUserViewModel = new AdminUserDto();
                adminUserViewModel.InjectFrom(src);

                adminUserViewModel.Password = null;
                adminUserViewModel.Roles = src.Roles.Select(s => s.Id).ToList();

                return adminUserViewModel;
            });

            Mapper.AddMap<AdminUserDto, AdminUser>((from, to) =>
            {
                var existing = to as AdminUser ?? new AdminUser();
                existing.InjectFrom(new LoopInjection(new[] { "Password" }), from);
                return existing;
            });
        }
    }
}
