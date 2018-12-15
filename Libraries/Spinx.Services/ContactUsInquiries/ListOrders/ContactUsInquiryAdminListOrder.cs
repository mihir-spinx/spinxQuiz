using Spinx.Domain.ContactUsInquiries;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.ContactUsInquiries.ListOrders
{
    public class ContactUsInquiryAdminListOrder : BaseListOrder<ContactUsInquiry>
    {
        public ContactUsInquiryAdminListOrder(IQueryable<ContactUsInquiry> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
        internal void Email()
        {
            Query = OrderBy(t => t.Email);
        }
        internal void Phone()
        {
            Query = OrderBy(t => t.Phone);
        }
        internal void CreatedAt()
        {
            Query = OrderBy(t => t.CreatedAt);
        }
    }
}