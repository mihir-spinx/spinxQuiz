using Spinx.Data.Infrastructure;
using Spinx.Domain.AdminRolePermissions;

namespace Spinx.Data.Repository.AdminRolePermissions
{
    public interface IAdminRoleRepository : IRepository<AdminRole>
    {
        
    }    

    public class AdminRoleRepository : Repository<AdminRole>, IAdminRoleRepository
    {
        public AdminRoleRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}