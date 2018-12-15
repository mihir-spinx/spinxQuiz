using Omu.ValueInjecter;
using Spinx.Domain.EmailTemplates;
using Spinx.Services.EmailTemplates.DTOs;

namespace Spinx.Services.EmailTemplates.Mappers
{
    public static class EmailTemplateAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<EmailTemplateCreateAdminDto, EmailTemplate>((from, to) =>
            {
                var existing = to as EmailTemplate ?? new EmailTemplate();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<EmailTemplateEditAdminDto, EmailTemplate>((from, to) =>
            {
                var existing = to as EmailTemplate ?? new EmailTemplate();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
