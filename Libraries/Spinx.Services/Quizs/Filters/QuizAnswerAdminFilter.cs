using Spinx.Domain.QuizAnswers;
using Spinx.Services.QuizAnswers.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.QuizAnswers.Filters
{
    public class QuizAnswerAdminFilter : BaseFilter<QuizAnswer, QuizAnswerAdminFilterDto>
    {
        public QuizAnswerAdminFilter(IQueryable<QuizAnswer> query, QuizAnswerAdminFilterDto dto) : base (query, dto) { }

        internal void Answer()
        {
            Query = Query.Where(w => w.Answer.Contains(Dto.Answer));
        }
        internal void QuizQuestionId()
        {
            Query = Query.Where(w => w.QuizQuestionId == Dto.QuizQuestionId);
        }
    }
}