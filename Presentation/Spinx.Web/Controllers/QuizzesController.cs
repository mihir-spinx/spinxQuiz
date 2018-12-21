using Spinx.Services.Members;
using Spinx.Services.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;
using Spinx.Services.QuizQuestions;
using Spinx.Services.Quizs;
using Spinx.Services.SeoPages;
using Spinx.Web.Infrastructure;
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
        private readonly int MemberId = 1;// Tempory
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
            var result = _memberQuizService.SaveMemberQuizInit(MemberId, slug);
            if(result.Success)
            {
                ViewBag.MemberQuizList = result.MemberQuizAnswerList;
                ViewBag.MemberResultId = result.MemberResultId;
                ViewBag.SortOrder = result.SortOrder;
                ViewBag.totelQuestion = result.MemberQuizAnswerList.Count;
                var entity = _quizService.GetQuizBySlug(slug);                

                return View(entity);
            }
            else           
                return HttpNotFound();
        }

        [HttpGet]
        [System.Web.Http.Route("api/quiz/getQuestion/")]
        public JsonNetResult GetQuestion(int memberResultId, int sortOrder)
        {
            var result = _memberQuizService.GetQuestionByMemberResult(memberResultId, sortOrder);
            return new JsonNetResult(result);
        }

        [HttpPost]
        [System.Web.Http.Route("api/quiz/getAnswer/")]
        public JsonNetResult GetAnswer(MemberQuizAnswerDto dto)
        {
            var result = _memberQuizService.SaveAnswerByMember(dto);
            return new JsonNetResult(result);
        }
        [HttpPost]
        [System.Web.Http.Route("api/quiz/submitquiz/")]
        public JsonNetResult SubmitQuiz(int memberResultId)
        {
            var result = _memberQuizService.SubmitQuiz(MemberId, memberResultId);
            result.SetRedirect( Url.RouteUrl("quizthankyou"));     
            // TODO : add logout logic
            return new JsonNetResult(result);
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}