using Spinx.Api.Infrastructure;
using Spinx.Services.AdminRolePermissions;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.List
{
    [AuthorizeApiAdminUser]
    public class AdminRolesController : BaseApiController
    {
        private readonly IAdminRoleService _adminRoleService;

        public AdminRolesController(
            IAdminRoleService adminRoleService)
        {
            _adminRoleService = adminRoleService;
        }

        public IHttpActionResult Get()
        {
            return Ok(_adminRoleService.GetAll());
        }
    }
}