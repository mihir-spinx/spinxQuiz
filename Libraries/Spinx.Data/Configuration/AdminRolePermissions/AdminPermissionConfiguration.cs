using Spinx.Domain.AdminRolePermissions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.AdminRolePermissions
{
    public class AdminPermissionConfiguration : EntityTypeConfiguration<AdminPermission>
    {
        public AdminPermissionConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_AdminPermissionUniqueName") { IsUnique = true }
                    ));

            Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

            Ignore(t => t.Depth);
        }
    }
}
