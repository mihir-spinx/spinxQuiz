namespace Spinx.Services.ContactUsInquiries.DTOs
{
    public class ContactUsDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Details { get; set; }
        public string gRecaptchaResponse { get; set; }
    }
}