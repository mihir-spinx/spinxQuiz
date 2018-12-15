using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class PagesController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"Pages"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"Pages.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"Pages.Edit"})]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}