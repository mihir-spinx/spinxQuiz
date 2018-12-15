using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class ErrorsController : BaseAdminController
    {
        public ActionResult NotFound()
        {
            return View();
        }
    }
}