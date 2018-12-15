using Spinx.Domain.QuizCategories;
using Spinx.Services.QuizCategories.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.QuizCategories.Filters
{
    public class QuizCategoryAdminFilter : BaseFilter<QuizCategory, QuizCategoryAdminFilterDto>
    {
        public QuizCategoryAdminFilter(IQueryable<QuizCategory> query, QuizCategoryAdminFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
        internal void IsActive()
        {
            Query = Query.Where(w => w.IsActive == Dto.IsActive);
        }
    }
}