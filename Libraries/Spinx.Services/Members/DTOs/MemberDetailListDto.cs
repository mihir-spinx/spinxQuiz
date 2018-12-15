using Spinx.Services.Infrastructure;
using System;

namespace Spinx.Services.Members.DTOs
{
    public class MemberDetailListDto : BaseFilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int? CreatedSource { get; set; }
        public string CreatedSourceName { get; set; }

        public decimal? TotalPayments { get; set; }
        public decimal? FromTotalPayment { get; set; }
        public decimal? ToTotalPayment { get; set; }

        public int? TotalJobPosts { get; set; }
        public decimal? JobPayment { get; set; }
        public int? ApprovedJobs { get; set; }
        public int? RejectedJobs { get; set; }
        public int? PendingJobs { get; set; }
        public int? PremiumJobs { get; set; }
        public int? ExpiredJobs { get; set; }
        public int? StarndardCredits { get; set; }
        public int? PremiumCredits { get; set; }

        public int? TotalAdvertisements { get; set; }
        public decimal? AdvertisementPayment { get; set; }
        public int? ApprovedAdvertisements { get; set; }
        public int? RejectedAdvertisements { get; set; }
        public int? PendingAdvertisements { get; set; }
        public int? ExpiredAdvertisements { get; set; }

        public int? TotalResumes { get; set; }
        public int? ApprovedResumes { get; set; }
        public int? RejectedResumes { get; set; }
        public int? PendingResumes { get; set; }
        
        public int? PurchasedResumesAccess { get; set; }
        public decimal? ResumeAccessPayment { get; set; }
        public int? AccessDaysLeft { get; set; }
    }
}