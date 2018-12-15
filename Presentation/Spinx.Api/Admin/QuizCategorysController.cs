using Spinx.Api.Infrastructure;
using Spinx.Services.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class QuizCategoriesController : BaseApiController
    {
        private readonly IQuizCategoryAdminService _quizCategoryAdminService;

        public QuizCategoriesController(
                    IQuizCategoryAdminService quizCategoryAdminService)
        {
            _quizCategoryAdminService = quizCategoryAdminService;
        }

        [AuthorizeApiAdminUser(permissions: new[] { "QuizCategories" })]
        public IHttpActionResult Get([FromUri]QuizCategoryAdminFilterDto dto)
        {
            return Result(_quizCategoryAdminService.List(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizCategories.Create"})]
        public IHttpActionResult Post([FromBody]QuizCategoryCreateAdminDto dto)
        {
            return Result(_quizCategoryAdminService.Create(dto));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizCategories.Edit"})]
        public IHttpActionResult Get(int id)
        {
            return Result(_quizCategoryAdminService.GetById(id));
        }

        [AuthorizeApiAdminUser(permissions: new [] {"QuizCategories.Edit"})]
        public IHttpActionResult Put(int id, [FromBody]QuizCategoryEditAdminDto dto)
        {
            return Result(_quizCategoryAdminService.Edit(id, dto));
        }
    }
}