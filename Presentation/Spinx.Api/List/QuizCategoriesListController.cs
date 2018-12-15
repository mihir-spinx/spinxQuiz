using Spinx.Api.Infrastructure;
using Spinx.Services.QuizCategories;
using System.Web.Http;

namespace Spinx.Api.List
{
    public class QuizCategoriesListController : BaseApiController
    {
        private readonly IQuizCategoryService _quizCategoryService;

        public QuizCategoriesListController(
            IQuizCategoryService quizCategoryService)
        {
            _quizCategoryService = quizCategoryService;
        }

        public IHttpActionResult Get()
        {
            return Ok(_quizCategoryService.Get());
        }
    }
}