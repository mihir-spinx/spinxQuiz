using Spinx.Api.Infrastructure;
using Spinx.Domain.QuizCategories;
using Spinx.Services.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;
using System.Web.Http;

namespace Spinx.Api.List
{
    public class QuizCategoriesFrontListController : BaseApiController
    {
        private readonly IQuizCategoryService _quizCategoryService;

        public QuizCategoriesFrontListController(
            IQuizCategoryService quizCategoryService)
        {
            _quizCategoryService = quizCategoryService;
        }
        public IHttpActionResult Get([FromUri]QuizCategory dto)
        {
            return Ok(_quizCategoryService.List(dto));
        }
    }
    
}