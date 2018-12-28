using Spinx.Services.Infrastructure;

namespace Spinx.Services.Members.DTOs
{
    public class MemberFilterDto : BaseFilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string College { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedSource { get; set; }
    }
}