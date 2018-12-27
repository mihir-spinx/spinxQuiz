using Spinx.Domain.Members;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members.DTOs;
using System.Linq;

namespace Spinx.Services.Members.Filters
{
    public class MemberResultListFilter : BaseFilter<MemberResult, MemberResultListDto>
    {

        public MemberResultListFilter(IQueryable<MemberResult> query, MemberResultListDto dto) : base(query, dto)
        {
        }

        internal void Name()
        {
            Query = Query.Where(w => w.Member.Name.Contains(Dto.Member.Name));
        }
        internal void Email()
        {
            Query = Query.Where(w => w.Member.Email.Contains(Dto.Member.Email));
        }
        internal void IsActive()
        {
            Query = Query.Where(w => w.Member.IsActive == Dto.Member.IsActive);
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