using Spinx.Core;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.AdminRolePermissions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.AdminRolePermissions.Actions
{
    public class AdminRoleActionFactory
    {
        private readonly IAdminRoleRepository _adminRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminRoleActionFactory(
            IAdminRoleRepository adminRoleRepository,
            IUnitOfWork unitOfWork)
        {
            _adminRoleRepository = adminRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public Result ExecuteAction(BaseFilterDto dto)
        {
            var result = new Result().SetSuccess();

            switch (dto.Action.NullSafeToLower())
            {
                case "delete": return new AdminRoleDeleteAction(_adminRoleRepository, _unitOfWork).ExecuteAction(dto.Ids);
            }

            return result;
        }
    }
}