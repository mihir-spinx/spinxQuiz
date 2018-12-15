using Spinx.Domain.SeoPages;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.SeoPages.ListOrders
{
    public class SeoPageAdminListOrder : BaseListOrder<SeoPage>
    {
        public SeoPageAdminListOrder(IQueryable<SeoPage> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
    }
}