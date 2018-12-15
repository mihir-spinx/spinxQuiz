using Spinx.Domain.QuizQuestions;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.QuizQuestions.ListOrders
{
    public class QuizQuestionAdminListOrder : BaseListOrder<QuizQuestion>
    {
        public QuizQuestionAdminListOrder(IQueryable<QuizQuestion> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Question()
        {
            Query = OrderBy(t => t.Question);
        }
        internal void SortOrder()
        {
            Query = OrderBy(t => t.SortOrder);
        }
        internal void CreatedAt()
        {
            Query = OrderBy(t => t.CreatedAt);
        }
        internal void IsActive()
        {
            Query = OrderBy(t => t.IsActive);
        }
    }
}