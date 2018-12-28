using Spinx.Domain.Members;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.Members.ListOrders
{
    public class MemberListOrder : BaseListOrder<Member>
    {
        //private readonly IPostAResumeRepository _postAResumeRepository;

        public MemberListOrder(IQueryable<Member> query, BaseFilterDto dto) : base(query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }

        internal void Email()
        {
            Query = OrderBy(t => t.Email);
        }

        internal void College()
        {
            Query = OrderBy(t => t.College);
        }


        internal void IsActive()
        {
            Query = OrderBy(t => t.IsActive);
        }

        internal void CreatedAt()
        {
            Query = OrderBy(o => o.CreatedAt);
        }
    }
}