using Spinx.Api.Infrastructure;
using Spinx.Services.QuizQuestions;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class QuizQuestionsSequenceController : BaseApiController
    {
        private readonly IQuizQuestionAdminService _quizQuestionAdminService;

        public QuizQuestionsSequenceController(
                    IQuizQuestionAdminService quizQuestionAdminService)
        {
            _quizQuestionAdminService = quizQuestionAdminService;
        }
        
        [AuthorizeAdminUser(permissions: new[] { "Application" })]
        public void Post([FromBody]int[] siblingIds)
        {
            _quizQuestionAdminService.SaveSortOrder(siblingIds);
        }
    }
}