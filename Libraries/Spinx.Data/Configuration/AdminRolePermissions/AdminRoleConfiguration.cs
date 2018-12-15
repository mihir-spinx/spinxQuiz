using Spinx.Domain.AdminRolePermissions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.AdminRolePermissions
{
    public class AdminRoleConfiguration : EntityTypeConfiguration<AdminRole>
    {
        public AdminRoleConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.SystemName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_AdminRoleUniqueSystemName") { IsUnique = true }
                    ));
        }
    }
}
