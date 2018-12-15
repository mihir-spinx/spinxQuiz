using Spinx.Data.Infrastructure;
using Spinx.Domain.ContactUsInquiries;

namespace Spinx.Data.Repository.ContactUsInquiries
{
    public interface IContactUsInquiryRepository : IRepository<ContactUsInquiry>
    {

    }

    public class ContactUsInquiryRepository : Repository<ContactUsInquiry>, IContactUsInquiryRepository
    {
        public ContactUsInquiryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}