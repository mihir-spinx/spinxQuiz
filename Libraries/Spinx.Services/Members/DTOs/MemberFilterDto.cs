using System;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.Members.DTOs
{
    public class MemberFilterDto : BaseFilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public int ApproveAdvertisement { get; set; }
        public int RejectedAdvertisement { get; set; }
        public int PendingAdvertisement { get; set; }
        public int ExpiredAdvertisement { get; set; }
        public DateTime? ResumeAccessExpiry{ get; set; }
        public int CreatedSource { get; set; }
    }
}