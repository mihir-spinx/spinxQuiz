using Spinx.Api.Infrastructure;
using Spinx.Services.QuizAnswers;
using Spinx.Services.QuizAnswers.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class QuizAnswersController : BaseApiController
    {
        private readonly IQuizAnswerAdminService _quizAnswerAdminService;

        public QuizAnswersController(
                    IQuizAnswerAdminService quizAnswerAdminService)
        {
            _quizAnswerAdminService = quizAnswerAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "QuizAnswers" })]
        public IHttpActionResult Get([FromUri]QuizAnswerAdminFilterDto dto)
        {
            return Result(_quizAnswerAdminService.List(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizAnswers.Create"})]
        public IHttpActionResult Post([FromBody]QuizAnswerCreateAdminDto dto)
        {
            return Result(_quizAnswerAdminService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizAnswers.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_quizAnswerAdminService.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizAnswers.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]QuizAnswerEditAdminDto dto)
        {
            return Result(_quizAnswerAdminService.Edit(id, dto));
        }
    }
}