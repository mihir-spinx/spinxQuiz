using System;
using Spinx.Services.Members;
using Spinx.Services.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;
using Spinx.Services.QuizQuestions;
using Spinx.Services.Quizs;
using Spinx.Services.SeoPages;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;
using Newtonsoft.Json;
using Spinx.Core;
using Spinx.Services.GeneralSettings;
using Spinx.Web.Core.Authentication;

namespace Spinx.Web.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IQuizCategoryService _quizCategoryService;
        private readonly IQuizQuestionService _quizQuestionService;
        private readonly ISeoPageService _seoPageService;
        private readonly IMemberQuizService _memberQuizService;
        private readonly IGeneralSettingService _generalSettingService;

        public QuizzesController(IQuizService quizService,
            IQuizCategoryService quizCategoryService,
            IQuizQuestionService quizQuestionService,
            IMemberQuizService memberQuizService,
            IGeneralSettingService generalSettingService,
            ISeoPageService seoPageService)
        {
            _quizService = quizService;
            _quizCategoryService = quizCategoryService;
            _quizQuestionService = quizQuestionService;
            _seoPageService = seoPageService;
            _memberQuizService = memberQuizService;
            _generalSettingService = generalSettingService;
        }

        public ActionResult Index()
        {
            if (!UserAuth.IsLogedIn())
                return RedirectToAction("Login", "Member");

            var entity = _seoPageService.GetPageMeta("Quizzes");
            if (entity == null) return View();

            //ViewBag.Title = entity.MetaTitle;
            //ViewBag.MetaDescription = entity.MetaDescription;

            return View();
        }

        public ActionResult Detail(string slug)
        {
            if (!UserAuth.IsLogedIn())
                return RedirectToAction("Login", "Member");

            var entity = _quizService.GetQuizBySlug(slug);

            var result = _memberQuizService.CheckQuizRunning(UserAuth.User.UserId, entity.Id);
            if(result.Success)
                return RedirectToAction("Question", "Quizzes");

            ViewBag.slug = slug;
            entity.QuizCategory = _quizCategoryService.GetQuizCategoryById(entity.QuizCategoryId);
            ViewBag.QuizQuestions = _quizQuestionService.GetQuizQuestionsByQuizId(entity.Id);

            if (entity == null)
                return HttpNotFound();

            return View(entity);
        }

        public ActionResult Question(string slug)
        {
            if (!UserAuth.IsLogedIn())
                return RedirectToAction("Login", "Member");

            var result = _memberQuizService.SaveMemberQuizInit(UserAuth.User.UserId, slug);
            if(result.Success)
            {
                ViewBag.MemberQuizList =  JsonConvert.SerializeObject(result.MemberQuizAnswerList);
                ViewBag.MemberResultId = result.MemberResultId;
                var TotalTimeData =  _generalSettingService.GetGeneralSetting("total-time");
                ViewBag.TotalTimeMinute = string.IsNullOrWhiteSpace(TotalTimeData) ? "60" : TotalTimeData;
                ViewBag.diffInSeconds = (DateTime.Now - result.StartTime).TotalSeconds;
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
        public JsonNetResult GetQuestion(int memberResultId, int sortOrder,int lastSortOrder)
        {
            if (!UserAuth.IsLogedIn())
                return  new JsonNetResult(new Result().SetError("Please Login"));
            var result = _memberQuizService.GetQuestionByMemberResult(memberResultId, sortOrder, lastSortOrder);
            return new JsonNetResult(result);
        }

        [HttpPost]
        [System.Web.Http.Route("api/quiz/getAnswer/")]
        public JsonNetResult GetAnswer(MemberQuizAnswerDto dto)
        {
            if (!UserAuth.IsLogedIn())
                return new JsonNetResult(new Result().SetError("Please Login"));
            var result = _memberQuizService.SaveAnswerByMember(dto);
            return new JsonNetResult(result);
        }
        [HttpPost]
        [System.Web.Http.Route("api/quiz/submitquiz/")]
        public JsonNetResult SubmitQuiz(int memberResultId)
        {
            if (!UserAuth.IsLogedIn())
                return new JsonNetResult(new Result().SetError("Please Login"));
            var result = _memberQuizService.SubmitQuiz(UserAuth.User.UserId, memberResultId);
            result.SetRedirect( Url.RouteUrl("quizthankyou"));
            
            UserAuth.Signout(UserAuth.CookieUser);

            return new JsonNetResult(result);
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}