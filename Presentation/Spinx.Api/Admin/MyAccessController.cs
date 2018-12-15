using Spinx.Api.Infrastructure;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class MyAccessController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            var result = new
            {
                roles = UserAuth.AdminUser.Roles,
                permissions = UserAuth.AdminUser.Permissions
            };
            return Ok(result);
        }
    }
}