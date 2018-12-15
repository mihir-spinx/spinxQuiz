using Spinx.Api.Infrastructure;
using Spinx.Services.QuizAnswers;
using Spinx.Web.Core.Authentication;
using System.Web.Http;

namespace Spinx.Api.Admin
{
    [AuthorizeApiAdminUser]
    public class QuizAnswerSequenceController : BaseApiController
    {
        private readonly IQuizAnswerAdminService _quizAnswerAdminService;

        public QuizAnswerSequenceController(
                    IQuizAnswerAdminService quizAnswerAdminService)
        {
            _quizAnswerAdminService = quizAnswerAdminService;
        }
        
        [AuthorizeAdminUser(permissions: new[] { "QuizAnswers.Sequence" })]
        public void Post([FromBody]int[] siblingIds)
        {
            _quizAnswerAdminService.SaveSortOrder(siblingIds);
        }
    }
}