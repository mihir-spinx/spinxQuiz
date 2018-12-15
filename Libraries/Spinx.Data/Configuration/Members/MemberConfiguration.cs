using Spinx.Domain.Members;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.Members
{
    public class MemberConfiguration : EntityTypeConfiguration<Member>
    {
        public MemberConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_MemberUniqueEmail") { IsUnique = true }
                    ));

            Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Salt)
                .IsRequired()
                .HasMaxLength(256);

            Property(t => t.ForgotPasswordToken)
                .HasMaxLength(50);

            Property(t => t.AddressLine1)
                .IsOptional()
                .HasMaxLength(256);

            Property(t => t.AddressLine2)
                .IsOptional()
                .HasMaxLength(256);

            Property(t => t.City)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.State)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.Phone)
                .IsOptional()
                .HasMaxLength(20);

            Property(t => t.College)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.Degree)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.LastSemMark)
                .IsOptional()
                .HasMaxLength(10);

            Property(t => t.Experience)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.UploadResume)
                .IsOptional()
                .HasMaxLength(250);
        }
    }
}