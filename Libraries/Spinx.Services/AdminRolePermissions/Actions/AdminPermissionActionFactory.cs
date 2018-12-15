using Spinx.Core;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.AdminRolePermissions.Actions
{
    public class AdminPermissionActionFactory
    {
        private readonly IAdminPermissionRepository _adminPermissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminPermissionActionFactory(
            IAdminPermissionRepository adminPermissionRepository,
            IUnitOfWork unitOfWork)
        {
            _adminPermissionRepository = adminPermissionRepository;
            _unitOfWork = unitOfWork;
        }

        public Result ExecuteAction(BaseFilterDto dto)
        {
            var result = new Result().SetSuccess();

            switch (dto.Action.NullSafeToLower())
            {
                case "delete": return new AdminPermissionDeleteAction(_adminPermissionRepository, _unitOfWork).ExecuteAction(dto.Ids);
            }

            return result;
        }
    }
}