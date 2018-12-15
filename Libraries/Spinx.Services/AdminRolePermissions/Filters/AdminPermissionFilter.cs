using Spinx.Domain.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.AdminRolePermissions.Filters
{
    public class AdminPermissionFilter : BaseFilter<AdminPermission, AdminPermissionFilterDto>
    {
        public AdminPermissionFilter(IQueryable<AdminPermission> query, AdminPermissionFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }

        internal void DisplayName()
        {
            Query = Query.Where(w => w.DisplayName.Contains(Dto.DisplayName));
        }
    }
}