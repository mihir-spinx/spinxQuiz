using Spinx.Domain.EmailTemplates;
using Spinx.Services.EmailTemplates.DTOs;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.EmailTemplates.Filters
{
    public class EmailTemplateAdminFilter : BaseFilter<EmailTemplate, EmailTemplateAdminFilterDto>
    {
        public EmailTemplateAdminFilter(IQueryable<EmailTemplate> query, EmailTemplateAdminFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
    }
}