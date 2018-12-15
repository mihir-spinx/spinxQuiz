using Spinx.Domain.ContactUsInquiries;
using Spinx.Services.Infrastructure;
using System.Linq;
using Spinx.Services.ContactUsInquiries.DTOs;

namespace Spinx.Services.ContactUsInquiries.Filters
{
    public class ContactUsInquiryAdminFilter : BaseFilter<ContactUsInquiry, ContactUsInquiryAdminFilterDto>
    {
        public ContactUsInquiryAdminFilter(IQueryable<ContactUsInquiry> query, ContactUsInquiryAdminFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
        internal void Email()
        {
            Query = Query.Where(w => w.Email.Contains(Dto.Email));
        }
        internal void Phone()
        {
            Query = Query.Where(w => w.Phone.Contains(Dto.Phone));
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