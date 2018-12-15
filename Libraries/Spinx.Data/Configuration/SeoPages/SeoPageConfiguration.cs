using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Spinx.Domain.SeoPages;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.SeoPages
{
    public class SeoPageConfiguration : EntityTypeConfiguration<SeoPage>
    {
        public SeoPageConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_SeoPageUniqueName") { IsUnique = true }
                    ));

            Property(t => t.MetaTitle)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.MetaDescription)
                .IsOptional()
                .HasMaxLength(500);
        }
    }
}