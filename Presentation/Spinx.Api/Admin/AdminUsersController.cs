using Spinx.Api.Infrastructure;
using Spinx.Services.AdminUsers;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class AdminUsersController : BaseApiController
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUsersController(
            IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "AdminUsers" })]
        public IHttpActionResult Get([FromUri]AdminUserFilterDto dto)
        {
            return Result(_adminUserService.List(dto, UserAuth.AdminUser.UserId));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"AdminUsers.Create"})]
        public IHttpActionResult Post([FromBody]AdminUserDto dto)
        {
            return Result(_adminUserService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"AdminUsers.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_adminUserService.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"AdminUsers.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]AdminUserDto dto)
        {
            return Result(_adminUserService.Edit(id, dto));
        }
    }
}
