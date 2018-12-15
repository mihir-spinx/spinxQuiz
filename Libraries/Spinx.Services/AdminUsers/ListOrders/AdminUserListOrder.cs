using Spinx.Domain.AdminUsers;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.AdminUsers.ListOrders
{
    public class AdminUserListOrder : BaseListOrder<AdminUser>
    {
        public AdminUserListOrder(IQueryable<AdminUser> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
        
        internal void Email()
        {
            Query = OrderBy(t => t.Email);
        }

        internal void IsActive()
        {
            Query = OrderBy(t => t.IsActive);
        }

        internal void LastLoginAt()
        {
            Query = OrderBy(o => o.LastLoginAt);
        }

        internal void CreatedAt()
        {
            Query = OrderBy(o => o.CreatedAt);
        }
    }
}