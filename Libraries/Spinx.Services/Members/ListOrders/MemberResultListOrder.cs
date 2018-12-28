using Spinx.Domain.Members;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.Members.ListOrders
{
    public class MemberResultListOrder : BaseListOrder<MemberResult>
    {
        public MemberResultListOrder(IQueryable<MemberResult> query, BaseFilterDto dto) : base(query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Member.Name);
        }

        internal void Email()
        {
            Query = OrderBy(t => t.Member.Email);
        }

        internal void College()
        {
            Query = OrderBy(w => w.Member.College);
        }

        internal void QuizTitle()
        {
            Query = OrderBy(w => w.Quiz.Title);
        }

        internal void QuizCategoryName()
        {
            Query = OrderBy(w => w.Quiz.QuizCategory.Name);
        }

        internal void Score()
        {
            Query = OrderBy(w => w.Score);
        }

        internal void Percentage()
        {
            Query = OrderBy(w => w.Percentage);
        }

        internal void IsActive()
        {
            Query = OrderBy(t => t.Member.IsActive);
        }

        internal void CreatedAt()
        {
            Query = OrderBy(t => t.CreatedAt);
        }
    }
}