using Spinx.Domain.AdminRolePermissions;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.AdminRolePermissions.ListOrders
{
    public class AdminRoleListOrder : BaseListOrder<AdminRole>
    {
        public AdminRoleListOrder(IQueryable<AdminRole> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
        
        internal void UsersCount()
        {
            Query = OrderBy(t => t.Users.Count);
        }
    }
}