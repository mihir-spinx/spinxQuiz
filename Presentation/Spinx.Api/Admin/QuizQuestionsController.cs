using Spinx.Api.Infrastructure;
using Spinx.Services.QuizQuestions;
using Spinx.Services.QuizQuestions.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class QuizQuestionsController : BaseApiController
    {
        private readonly IQuizQuestionAdminService _quizQuestionAdminService;

        public QuizQuestionsController(
                    IQuizQuestionAdminService quizQuestionAdminService)
        {
            _quizQuestionAdminService = quizQuestionAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "QuizQuestions" })]
        public IHttpActionResult Get([FromUri]QuizQuestionAdminFilterDto dto)
        {
            return Result(_quizQuestionAdminService.List(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizQuestions.Create"})]
        public IHttpActionResult Post([FromBody]QuizQuestionCreateAdminDto dto)
        {
            return Result(_quizQuestionAdminService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizQuestions.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_quizQuestionAdminService.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizQuestions.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]QuizQuestionEditAdminDto dto)
        {
            return Result(_quizQuestionAdminService.Edit(id, dto));
        }
    }
}