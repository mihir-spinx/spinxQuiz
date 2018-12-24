﻿using Spinx.Services.Members;
using Spinx.Services.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;
using Spinx.Services.QuizQuestions;
using Spinx.Services.Quizs;
using Spinx.Services.SeoPages;
using Spinx.Web.Infrastructure;
using System.Web.Mvc;
using Spinx.Core;
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
            if (!UserAuth.IsLogedIn())
                return  new JsonNetResult(new Result().SetError("Please Login"));
            var result = _memberQuizService.GetQuestionByMemberResult(memberResultId, sortOrder);
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