using Spinx.Web.Core.Authentication;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;

namespace Spinx.Web.Areas.Admin.Controllers
{
    [AuthorizeAdminUser]
    public class QuizQuestionsController : BaseAdminController
    {
        [AuthorizeAdminUser(permissions: new [] {"QuizQuestions"})]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"QuizQuestions.Create"})]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeAdminUser(permissions: new [] {"QuizQuestions.Edit"})]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}