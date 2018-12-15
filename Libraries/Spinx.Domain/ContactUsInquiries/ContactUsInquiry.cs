using Spinx.Core.Domain;
using System;

namespace Spinx.Domain.ContactUsInquiries
{
    public class ContactUsInquiry : IModificationHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}