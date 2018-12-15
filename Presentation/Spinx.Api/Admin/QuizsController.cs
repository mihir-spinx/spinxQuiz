using Spinx.Api.Infrastructure;
using Spinx.Services.Quizs;
using Spinx.Services.Quizs.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class QuizsController : BaseApiController
    {
        private readonly IQuizAdminService _quizAdminService;

        public QuizsController(
                    IQuizAdminService quizAdminService)
        {
            _quizAdminService = quizAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "Quizs" })]
        public IHttpActionResult Get([FromUri]QuizAdminFilterDto dto)
        {
            return Result(_quizAdminService.List(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Quizs.Create"})]
        public IHttpActionResult Post([FromBody]QuizCreateAdminDto dto)
        {
            return Result(_quizAdminService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Quizs.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_quizAdminService.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"Quizs.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]QuizEditAdminDto dto)
        {
            return Result(_quizAdminService.Edit(id, dto));
        }
    }
}