using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Services.Content;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.AdminUsers.Actions
{
    public class AdminUserActiveAction
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminUserActiveAction(
            IAdminUserRepository adminUserRepository, 
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Result ExecuteAction(List<int> ids)
        {
            var query = _adminUserRepository.AsNoTracking.Where(q => ids.Contains(q.Id));

            var result = new Result().SetSuccess(string.Format(Messages.RecordActivate, query.Count()));

            foreach (var entity in query)
            {
                entity.IsActive = true;
                _adminUserRepository.Update(entity);
            }

            _unitOfWork.Commit();
            AdminUserCacheManager.ClearCache();

            return result;
        }
    }
}