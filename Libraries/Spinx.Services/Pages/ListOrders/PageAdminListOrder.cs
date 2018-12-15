using Spinx.Domain.Pages;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.Pages.ListOrders
{
    public class PageAdminListOrder : BaseListOrder<Page>
    {
        public PageAdminListOrder(IQueryable<Page> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Title()
        {
            Query = OrderBy(t => t.Title);
        }
        
        internal void IsActive()
        {
            Query = OrderBy(t => t.IsActive);
        }
    }
}