using Spinx.Domain.Quizs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.Quizs.ListOrders
{
    public class QuizAdminListOrder : BaseListOrder<Quiz>
    {
        public QuizAdminListOrder(IQueryable<Quiz> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Title()
        {
            Query = OrderBy(t => t.Title);
        }
        internal void SortOrder()
        {
            Query = OrderBy(t => t.SortOrder);
        }
        internal void IsActive()
        {
            Query = OrderBy(t => t.IsActive);
        }
        internal void CreatedAt()
        {
            Query = OrderBy(t => t.CreatedAt);
        }
        internal void QuizCategoryName()
        {
            Query = OrderBy(t => t.QuizCategory.Name);
        }
    }
}