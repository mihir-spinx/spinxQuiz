using Spinx.Core;
using System.Linq;
using System.Web.Mvc;

namespace Spinx.Web.Infrastructure
{
    public class BaseController : Controller
    {
        //public IComponentContext IocContext { get; set; }
        public BaseController()
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected JsonResult ToJsonResult(Result result, int statusCode = 200)
        {
            HttpContext.Response.StatusCode = result.Errors.Any() ? 422 : statusCode;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}