using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class QuizAnswersController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"QuizAnswers"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"QuizAnswers.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"QuizAnswers.Edit"})]
        public ActionResult Edit(int Id)
        {
            return View();
        }
    }
}