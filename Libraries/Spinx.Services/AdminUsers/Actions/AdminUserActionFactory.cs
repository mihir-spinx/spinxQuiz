using Spinx.Core;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminUsers;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.AdminUsers.Actions
{
    public class AdminUserActionFactory
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminUserActionFactory(
            IAdminUserRepository adminUserRepository,
            IUnitOfWork unitOfWork)
        {
            _adminUserRepository = adminUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Result ExecuteAction(BaseFilterDto dto, int loggedInUserId)
        {
            var result = new Result().SetSuccess();

            if (string.IsNullOrEmpty(dto?.Action)) return new Result();

            switch (dto.Action.NullSafeToLower())
            {
                case "active": return new AdminUserActiveAction(_adminUserRepository, _unitOfWork).ExecuteAction(dto.Ids);
                case "inactive": return new AdminUserInactiveAction(_adminUserRepository, _unitOfWork).ExecuteAction(dto.Ids, loggedInUserId);
                case "delete": return new AdminUserDeleteAction(_adminUserRepository, _unitOfWork).ExecuteAction(dto.Ids, loggedInUserId);
            }

            return result;
        }
    }
}