using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.AdminRolePermissions.Actions
{
    public class AdminPermissionDeleteAction
    {
        private readonly IAdminPermissionRepository _adminPermissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private Result _result;

        public AdminPermissionDeleteAction(
            IAdminPermissionRepository adminPermissionRepository,
            IUnitOfWork unitOfWork)
        {
            _adminPermissionRepository = adminPermissionRepository;
            _unitOfWork = unitOfWork;

            _result = new Result();
        }

        private void Validation(ICollection<int> ids)
        {
            if (_adminPermissionRepository.AsNoTracking.Any(w => ids.Contains(w.Id) && w.Children.Count > 0))
                _result.SetError("You can't delete any record(s) which contain child permission.");

            if (_adminPermissionRepository.AsNoTracking.Any(w => ids.Contains(w.Id) && w.AdminRoles.Count > 0))
                _result.SetError("You can't delete any record(s) which are assigned to admin roles.");
        }

        public Result ExecuteAction(List<int> ids)
        {
            Validation(ids);
            if (!_result.Success) return _result;

            var query = _adminPermissionRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            _result = new Result().SetSuccess($"Total {query.Count()} record(s) has been deleted.");

            foreach (var entity in query)
                _adminPermissionRepository.Delete(entity);

            _unitOfWork.Commit();
            AdminPermissionCacheManager.ClearCache();

            return _result;
        }
    }
}