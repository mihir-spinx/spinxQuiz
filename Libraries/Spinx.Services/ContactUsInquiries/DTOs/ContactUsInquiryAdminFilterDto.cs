using Spinx.Services.Infrastructure;

namespace Spinx.Services.ContactUsInquiries.DTOs
{
    public class ContactUsInquiryAdminFilterDto: BaseFilterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}