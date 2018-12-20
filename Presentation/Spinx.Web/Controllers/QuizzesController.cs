using Spinx.Services.Members;
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
        private readonly IMemberQuizService _memberQuizService;
        
        public QuizzesController(IQuizService quizService,
            IQuizCategoryService quizCategoryService,
            IQuizQuestionService quizQuestionService,
            IMemberQuizService memberQuizService,
            ISeoPageService seoPageService)
        {
            _quizService = quizService;
            _quizCategoryService = quizCategoryService;
            _quizQuestionService = quizQuestionService;
            _seoPageService = seoPageService;
            _memberQuizService = memberQuizService;
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
            ViewBag.slug = slug;
            entity.QuizCategory = _quizCategoryService.GetQuizCategoryById(entity.QuizCategoryId);
            ViewBag.QuizQuestions = _quizQuestionService.GetQuizQuestionsByQuizId(entity.Id);

            if (entity == null)
                return HttpNotFound();

            return View(entity);
        }

        public ActionResult Question(string slug)
        {
            var result = _memberQuizService.SaveMemberQuizInit(1, slug);

            var entity = _quizService.GetQuizBySlug(slug);

            entity.QuizCategory = _quizCategoryService.GetQuizCategoryById(entity.QuizCategoryId);
            ViewBag.QuizQuestions = _quizQuestionService.GetQuizQuestionsByQuizId(entity.Id);

            if (entity == null)
                return HttpNotFound();

            return View(entity);
        }
    }
}