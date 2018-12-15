using Spinx.Domain.QuizCategories;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.QuizCategories.ListOrders
{
    public class QuizCategoryAdminListOrder : BaseListOrder<QuizCategory>
    {
        public QuizCategoryAdminListOrder(IQueryable<QuizCategory> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
        internal void SortOrder()
        {
            Query = OrderBy(t => t.SortOrder);
        }
        internal void IsActive()
        {
            Query = OrderBy(t => t.IsActive);
        }
    }
}