using Spinx.Domain.Members;
using Spinx.Services.Members.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.Members.Filters
{
    public class MemberFilter : BaseFilter<Member, MemberFilterDto>
    {
        public MemberFilter(IQueryable<Member> query, MemberFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
        internal void Email()
        {
            Query = Query.Where(w => w.Email.Contains(Dto.Email));
        }
        internal void IsActive()
        {
            Query = Query.Where(w => w.IsActive == Dto.IsActive);
        }
        internal void FromCreatedAt()
        {
            Query = Query.Where(w => w.CreatedAt >= Dto.FromCreatedAt);
        }
        internal void ToCreatedAt()
        {
            Query = Query.Where(w => w.CreatedAt <= Dto.ToCreatedAt);
        }
    }
}