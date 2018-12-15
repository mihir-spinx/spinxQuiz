using Spinx.Services.Infrastructure;

namespace Spinx.Services.Pages.DTOs
{
    public class PageAdminFilterDto: BaseFilterDto
    {
        public int SiteId { get; set; }

        public string Title { get; set; }
        public bool? IsActive { get; set; }
    }
}