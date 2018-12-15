using Spinx.Domain.Pages;
using Spinx.Services.Infrastructure;
using Spinx.Services.Pages.DTOs;
using System.Linq;

namespace Spinx.Services.Pages.Filters
{
    public class PageAdminFilter : BaseFilter<Page, PageAdminFilterDto>
    {
        public PageAdminFilter(IQueryable<Page> query, PageAdminFilterDto dto) : base (query, dto) { }

        internal void Title()
        {
            Query = Query.Where(w => w.Title.Contains(Dto.Title));
        }

        internal void IsActive()
        {
            Query = Query.Where(w => w.IsActive == Dto.IsActive);
        }
    }
}