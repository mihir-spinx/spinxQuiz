using Spinx.Domain.EmailTemplates;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.EmailTemplates.ListOrders
{
    public class EmailTemplateAdminListOrder : BaseListOrder<EmailTemplate>
    {
        public EmailTemplateAdminListOrder(IQueryable<EmailTemplate> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
    }
}