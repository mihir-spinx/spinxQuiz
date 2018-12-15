using Spinx.Services.QuizCategories;
using Spinx.Services.QuizQuestions;
using Spinx.Services.Quizs;
using Spinx.Services.SeoPages;
using System.Web.Mvc;

namespace Spinx.Web.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IQuizCategoryService _quizCategoryService;
        private readonly IQuizQuestionService _quizQuestionService;
        private readonly ISeoPageService _seoPageService;

        public QuizzesController(IQuizService quizService,
            IQuizCategoryService quizCategoryService,
            IQuizQuestionService quizQuestionService,
            ISeoPageService seoPageService)
        {
            _quizService = quizService;
            _quizCategoryService = quizCategoryService;
            _quizQuestionService = quizQuestionService;
            _seoPageService = seoPageService;
        }

        public ActionResult Index()
        {
            var entity = _seoPageService.GetPageMeta("Quizzes");
            if (entity == null) return View();

            //ViewBag.Title = entity.MetaTitle;
            //ViewBag.MetaDescription = entity.MetaDescription;

            return View();
        }

        public ActionResult Detail(string slug)
        {
            var entity = _quizService.GetQuizBySlug(slug);

            entity.QuizCategory = _quizCategoryService.GetQuizCategoryById(entity.QuizCategoryId);
            ViewBag.QuizQuestions = _quizQuestionService.GetQuizQuestionsByQuizId(entity.Id);

            if (entity == null)
                return HttpNotFound();

            return View(entity);
        }
    }
}