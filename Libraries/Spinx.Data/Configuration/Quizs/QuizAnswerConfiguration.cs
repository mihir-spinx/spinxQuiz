using Spinx.Domain.QuizAnswers;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.QuizAnswers
{
    public class QuizAnswerConfiguration : EntityTypeConfiguration<QuizAnswer>
    {
        public QuizAnswerConfiguration()
        {
            Property(t => t.QuizQuestionId)
                .IsRequired();

            Property(t => t.Answer)
                .IsRequired()
                .HasMaxLength(500);

            Property(t => t.IsCorrectAnswer)
                .IsRequired();
        }
    }
}