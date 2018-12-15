using Spinx.Domain.QuizQuestions;
using Spinx.Services.QuizQuestions.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.QuizQuestions.Filters
{
    public class QuizQuestionAdminFilter : BaseFilter<QuizQuestion, QuizQuestionAdminFilterDto>
    {
        public QuizQuestionAdminFilter(IQueryable<QuizQuestion> query, QuizQuestionAdminFilterDto dto) : base (query, dto) { }

        internal void Question()
        {
            Query = Query.Where(w => w.Question.Contains(Dto.Question));
        }
        internal void IsActive()
        {
            Query = Query.Where(w => w.IsActive == Dto.IsActive);
        }
        internal void QuizId()
        {
            Query = Query.Where(w => w.QuizId == Dto.QuizId);
        }
    }
}