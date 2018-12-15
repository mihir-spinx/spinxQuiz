using Spinx.Data.Infrastructure;
using Spinx.Domain.AdminRolePermissions;
using System.Data.Entity;
using System.Linq;

namespace Spinx.Data.Repository.AdminRolePermissions
{
    public interface IAdminPermissionRepository : IRepository<AdminPermission>, INestedSet
    {
        
    }  

    public class AdminPermissionRepository : Repository<AdminPermission>, IAdminPermissionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<AdminPermission> _dbSet;

        public AdminPermissionRepository(IDatabaseFactory databaseFactory, IUnitOfWork unitOfWork) : base(databaseFactory)
        {
            _unitOfWork = unitOfWork;
            _dbSet = databaseFactory.Get().Set<AdminPermission>();
        }

        public new void Insert(AdminPermission entity)
        {
            // When no parent selected
            if (entity.ParentId == null)
            {
                var maxNestedSet = AsNoTracking.Max(c => c.Right) ?? 0;
                entity.Left = maxNestedSet + 1;
                entity.Right = maxNestedSet + 2;
                entity.Depth = 0;
            }
            else
            {
                var parentAdminPermission = AsNoTracking.First(w => w.Id == entity.ParentId);
                var valNode = (parentAdminPermission.Left + 1 == parentAdminPermission.Right)
                    ? parentAdminPermission.Left
                    : parentAdminPermission.Right - 1;

                var rightNodes = _dbSet.Where(f=> f.Right > valNode).ToList();
                rightNodes.ForEach(c => c.Right = c.Right + 2);

                var leftNodes = _dbSet.Where(f=> f.Left > valNode).ToList();
                leftNodes.ForEach(c => c.Left = c.Left + 2);

                entity.Left = valNode + 1;
                entity.Right = valNode + 2;
                entity.Depth = parentAdminPermission.Depth + 1;
            }

            _dbSet.Add(entity);
            _unitOfWork.Commit();
        }

        public new void Delete(AdminPermission entity)
        {
            this.DeleteNode("AdminPermissions", entity.Id);
        }
    }
}