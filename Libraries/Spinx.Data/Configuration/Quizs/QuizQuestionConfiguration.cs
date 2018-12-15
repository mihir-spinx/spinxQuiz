using Spinx.Domain.QuizQuestions;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.QuizQuestions
{
    public class QuizQuestionConfiguration : EntityTypeConfiguration<QuizQuestion>
    {
        public QuizQuestionConfiguration()
        {
            Property(t => t.QuizId)
             .IsRequired();

            Property(t => t.Question)
                .IsRequired()
                .HasMaxLength(240);
        }
    }
}