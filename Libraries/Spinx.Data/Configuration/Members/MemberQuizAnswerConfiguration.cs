using Spinx.Domain.Members;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.Members
{
    public class MemberQuizAnswerConfiguration : EntityTypeConfiguration<MemberQuizAnswer>
    {
        public MemberQuizAnswerConfiguration()
        {
            Property(t => t.QuizAnswerId)
               .IsOptional();

            Property(t => t.IsRight)
               .IsOptional();

        }
    }
}
