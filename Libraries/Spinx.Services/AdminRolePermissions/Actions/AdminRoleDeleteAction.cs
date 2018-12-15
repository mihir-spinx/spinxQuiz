using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Spinx.Services.AdminRolePermissions.Actions
{
    public class AdminRoleDeleteAction
    {
        private readonly IAdminRoleRepository _adminRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private Result _result;

        public AdminRoleDeleteAction(
            IAdminRoleRepository adminRoleRepository,
            IUnitOfWork unitOfWork)
        {
            _adminRoleRepository = adminRoleRepository;
            _unitOfWork = unitOfWork;

            _result = new Result();
        }

        private void Validation(ICollection<int> ids)
        {
            if (_adminRoleRepository.AsNoTracking.Any(w => ids.Contains(w.Id) && w.Users.Count > 0))
                _result.SetError("You can't delete any role(s) which are assigned to any user.");
        }

        public Result ExecuteAction(List<int> ids)
        {
            Validation(ids);
            if (!_result.Success) return _result;

            var query = _adminRoleRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            _result = new Result().SetSuccess($"Total {query.Count()} record(s) has been deleted.");

            foreach (var entity in query)
                _adminRoleRepository.Delete(entity);

            _unitOfWork.Commit();
            AdminRoleCacheManager.ClearCache();

            return _result;
        }
    }
}