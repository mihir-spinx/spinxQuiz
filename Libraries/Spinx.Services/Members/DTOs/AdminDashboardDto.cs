using Spinx.Domain.ContactUsInquiries;
using System.Collections.Generic;

namespace Spinx.Services.Members.DTOs
{
    public class AdminDashboardDto
    {
        public int? TotalMembers { get; set; }
        public int? TotalContactUsInquiries { get; set; }

        public IList<ContactUsInquiry> RecentContactUs { get; set; }
        public int? TotalQuizQuestions { get; set; }
    }
}