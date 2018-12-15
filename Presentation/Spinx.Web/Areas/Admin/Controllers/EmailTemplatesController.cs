using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class EmailTemplatesController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"EmailTemplates"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"EmailTemplates.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"EmailTemplates.Edit"})]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}