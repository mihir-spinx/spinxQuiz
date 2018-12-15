using Spinx.Domain.AdminUsers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.AdminUsers
{
    public class AdminUserConfiguration : EntityTypeConfiguration<AdminUser>
    {
        public AdminUserConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_AdminUserUniqueEmail") { IsUnique = true }
                    ));

            Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.Salt)
                .IsRequired()
                .HasMaxLength(128);

            Property(t => t.ForgotPasswordToken)
                .HasMaxLength(256);
        }
    }
}