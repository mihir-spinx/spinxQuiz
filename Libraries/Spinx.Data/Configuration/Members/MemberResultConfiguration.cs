using Spinx.Domain.Members;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.Members
{
    public class MemberResultConfiguration : EntityTypeConfiguration<MemberResult>
    {
        public MemberResultConfiguration()
        {
            Property(t => t.AttempedQues)
                .IsOptional();

            Property(t => t.Score)
                .IsOptional();

            Property(t => t.Percentage)
                .IsOptional()
                .HasPrecision(9, 2);

            Property(t => t.TimeDuration)
                .IsOptional();
        }
    }
}