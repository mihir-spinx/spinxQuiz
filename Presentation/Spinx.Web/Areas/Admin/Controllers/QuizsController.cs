using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class QuizsController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"Quizs"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"Quizs.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"Quizs.Edit"})]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}