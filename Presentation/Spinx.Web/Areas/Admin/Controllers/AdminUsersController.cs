using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class AdminUsersController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"AdminUsers"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"AdminUsers.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"AdminUsers.Edit"})]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}