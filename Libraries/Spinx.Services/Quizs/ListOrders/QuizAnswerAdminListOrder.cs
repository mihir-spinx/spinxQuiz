using Spinx.Domain.QuizAnswers;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.QuizAnswers.ListOrders
{
    public class QuizAnswerAdminListOrder : BaseListOrder<QuizAnswer>
    {
        public QuizAnswerAdminListOrder(IQueryable<QuizAnswer> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Answer()
        {
            Query = OrderBy(t => t.Answer);
        }
        internal void SortOrder()
        {
            Query = OrderBy(t => t.SortOrder);
        }
    }
}