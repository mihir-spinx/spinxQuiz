using Spinx.Data.Infrastructure;
using Spinx.Domain.AdminUsers;
using System.Linq;

namespace Spinx.Data.Repository.AdminUsers
{
    public interface IAdminUserRepository : IRepository<AdminUser>
    {
        AdminUser FindActiveUserByEmail(string email);
    }

    public class AdminUserRepository : Repository<AdminUser>, IAdminUserRepository
    {
        public AdminUserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }

        public AdminUser FindActiveUserByEmail(string email)
        {
            return AsNoTracking.FirstOrDefault(t => t.Email == email && t.IsActive);
        }
    }
}