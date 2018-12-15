using Spinx.Domain.SeoPages;
using Spinx.Services.Infrastructure;
using System.Linq;
using Spinx.Services.SeoPages.DTOs;

namespace Spinx.Services.SeoPages.Filters
{
    public class SeoPageAdminFilter : BaseFilter<SeoPage, SeoPageAdminFilterDto>
    {
        public SeoPageAdminFilter(IQueryable<SeoPage> query, SeoPageAdminFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
    }
}