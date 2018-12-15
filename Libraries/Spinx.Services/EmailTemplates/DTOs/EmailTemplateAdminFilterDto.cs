using Spinx.Services.Infrastructure;

namespace Spinx.Services.EmailTemplates.DTOs
{
    public class EmailTemplateAdminFilterDto: BaseFilterDto
    {
        public string Name { get; set; }
    }
}