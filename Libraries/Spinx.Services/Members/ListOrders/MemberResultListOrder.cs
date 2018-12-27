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