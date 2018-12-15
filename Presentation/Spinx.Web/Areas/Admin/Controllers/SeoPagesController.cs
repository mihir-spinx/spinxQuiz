using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class SeoPagesController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"SeoPages"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"SeoPages.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"SeoPages.Edit"})]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}