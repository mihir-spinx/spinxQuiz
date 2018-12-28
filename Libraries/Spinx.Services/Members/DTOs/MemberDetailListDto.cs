using Spinx.Services.Infrastructure;
using System;

namespace Spinx.Services.Members.DTOs
{
    public class MemberDetailListDto : BaseFilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string College { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int CreatedSource { get; set; }



    }
}