using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Spinx.Domain.GeneralSettings;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.GeneralSettings
{
    public class GeneralSettingConfiguration : EntityTypeConfiguration<GeneralSetting>
    {
        public GeneralSettingConfiguration()
        {
            Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_GeneralSettingUniqueName") { IsUnique = true }
                    ));
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}