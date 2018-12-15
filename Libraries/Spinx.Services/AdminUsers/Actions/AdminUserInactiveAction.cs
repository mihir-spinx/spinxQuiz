using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.AdminUsers.Actions
{
    public class AdminUserInactiveAction
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminUserInactiveAction(
            IAdminUserRepository adminUserRepository, 
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;
        }

        private static Result Validation(ICollection<int> ids, int loggedInUserId)
        {
            return ids.Contains(loggedInUserId)
                ? new Result().SetError("You can't inactive your own account.")
                : new Result();
        }

        public Result ExecuteAction(List<int> ids, int loggedInUserId)
        {
            var result = Validation(ids, loggedInUserId);
            if (!result.Success) return result;

            var query = _adminUserRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            result = new Result().SetSuccess($"Total {query.Count()} record(s) has been deactivate.");

            foreach (var entity in query)
            {
                entity.IsActive = false;
                _adminUserRepository.Update(entity);
            }
                
            _unitOfWork.Commit();
            AdminUserCacheManager.ClearCache();

            return result;
        }
    }
}