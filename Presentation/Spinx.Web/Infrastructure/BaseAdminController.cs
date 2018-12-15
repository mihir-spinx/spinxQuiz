using Spinx.Core;
using System.Linq;
using System.Web.Mvc;

namespace Spinx.Web.Infrastructure
{
    public class BaseAdminController : Controller
    {
        public BaseAdminController()
        {

        }

        protected JsonResult ToJsonResult(Result result, int statusCode = 200)
        {
            //HttpContext.Response.StatusCode = result.Errors.Any() ? 422 : statusCode;
            HttpContext.Response.StatusCode = statusCode;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected RedirectToRouteResult PageNotFound()
        {
            return RedirectToAction("NotFound", "AdminErrors");
        }
    }
}