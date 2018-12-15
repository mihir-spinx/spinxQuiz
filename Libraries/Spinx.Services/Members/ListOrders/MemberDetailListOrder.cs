using Spinx.Domain.Members;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.Members.ListOrders
{
    public class MemberDetailListOrder : BaseListOrder<Member>
    {
       // private readonly IPaymentTransactionRepository _paymentTransactionRepository;

        public MemberDetailListOrder(IQueryable<Member> query, BaseFilterDto dto) : base(query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
        internal void Email()
        {
            Query = OrderBy(t => t.Email);
        }
        internal void IsActive()
        {
            Query = OrderBy(t => t.IsActive);
        }
        internal void CreatedAt()
        {
            Query = OrderBy(o => o.CreatedAt);
        }
        internal void CreatedSource()
        {
            Query = OrderBy(o => o.CreatedSource);
        }

        //internal void TotalPayments()
        //{
        //    Query = OrderBy(o => _paymentTransactionRepository.AsNoTracking.Sum(z => z.Payment));
        //}
    }
}