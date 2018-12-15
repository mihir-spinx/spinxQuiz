using Spinx.Domain.Quizs;
using Spinx.Services.Infrastructure;
using Spinx.Services.Quizs.DTOs;
using System.Linq;

namespace Spinx.Services.Quizs.Filters
{
    public class QuizAdminFilter : BaseFilter<Quiz, QuizAdminFilterDto>
    {
        public QuizAdminFilter(IQueryable<Quiz> query, QuizAdminFilterDto dto) : base (query, dto) { }

        internal void Title()
        {
            Query = Query.Where(w => w.Title.Contains(Dto.Title));
        }
        internal void IsActive()
        {
            Query = Query.Where(w => w.IsActive == Dto.IsActive);
        }
        internal void QuizCategoryId()
        {
            Query = Query.Where(w => w.QuizCategoryId == Dto.QuizCategoryId);
        }
    }
}