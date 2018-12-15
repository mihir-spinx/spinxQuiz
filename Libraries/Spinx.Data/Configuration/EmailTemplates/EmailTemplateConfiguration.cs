using Spinx.Domain.EmailTemplates;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.EmailTemplates
{
    public class EmailTemplateConfiguration : EntityTypeConfiguration<EmailTemplate>
    {
        public EmailTemplateConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Content)
                .IsRequired();
        }
    }
}