using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Spinx.Services.AdminUsers.Actions
{
    public class AdminUserDeleteAction
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private Result _result;

        public AdminUserDeleteAction(
            IAdminUserRepository adminUserRepository, 
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;

            _result = new Result();
        }

        private void Validation(ICollection<int> ids, int loggedInUserId)
        {
            if (ids.Contains(loggedInUserId))
                _result.SetError("You can't delete your own account.");
        }

        public Result ExecuteAction(List<int> ids, int loggedInUserId)
        {
            Validation(ids, loggedInUserId);
            if (!_result.Success) return _result;

            var query = _adminUserRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            _result = new Result().SetSuccess($"Total {query.Count()} record(s) has been deleted.");

            foreach (var entity in query)
                _adminUserRepository.Delete(entity);

            _unitOfWork.Commit();
            AdminUserCacheManager.ClearCache();

            return _result;
        }
    }
}