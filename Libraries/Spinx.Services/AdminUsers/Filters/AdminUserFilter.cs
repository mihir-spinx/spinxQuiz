using Spinx.Domain.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.AdminUsers.Filters
{
    public class AdminUserFilter : BaseFilter<AdminUser, AdminUserFilterDto>
    {
        public AdminUserFilter(IQueryable<AdminUser> query, AdminUserFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }

        internal void Email()
        {
            Query = Query.Where(w => w.Email.Contains(Dto.Email));
        }

        internal void RoleId()
        {
            Query = Query.Where(w => w.Roles.Any(r => r.Id == Dto.RoleId));
        }

        internal void IsActive()
        {
            Query = Query.Where(w => w.IsActive == Dto.IsActive);
        }

        internal void FromLastLoginAt()
        {
            Query = Query.Where(w => w.LastLoginAt >= Dto.FromLastLoginAt);
        }

        internal void ToLastLoginAt()
        {
            Query = Query.Where(w => w.LastLoginAt <= Dto.ToLastLoginAt);
        }

        internal void FromCreatedAt()
        {
            Query = Query.Where(w => w.CreatedAt >= Dto.FromCreatedAt);
        }

        internal void ToCreatedAt()
        {
            Query = Query.Where(w => w.CreatedAt <= Dto.ToCreatedAt);
        }
    }
}