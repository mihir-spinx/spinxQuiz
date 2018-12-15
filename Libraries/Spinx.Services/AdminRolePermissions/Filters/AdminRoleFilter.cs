using Spinx.Domain.AdminRolePermissions;
using Spinx.Services.AdminRolePermissions.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.AdminRolePermissions.Filters
{
    public class AdminRoleFilter : BaseFilter<AdminRole, AdminRoleFilterDto>
    {
        public AdminRoleFilter(IQueryable<AdminRole> query, AdminRoleFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
    }
}