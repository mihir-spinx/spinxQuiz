using Spinx.Domain.Members;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members.DTOs;
using System.Linq;

namespace Spinx.Services.Members.Filters
{
    public class MemberDetailListFilter : BaseFilter<Member, MemberDetailListDto>
    {
        //private readonly IPaymentTransactionRepository _paymentTransactionRepository;

        public MemberDetailListFilter(IQueryable<Member> query, MemberDetailListDto dto) : base(query, dto)
        {
        }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
        internal void Email()
        {
            Query = Query.Where(w => w.Email.Contains(Dto.Email));
        }
        internal void College()
        {
            Query = Query.Where(w => w.College.Contains(Dto.College));
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