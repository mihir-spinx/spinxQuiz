using Spinx.Domain.Quizs;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.Quizs
{
    public class QuizConfiguration : EntityTypeConfiguration<Quiz>
    {
        public QuizConfiguration()
        {
            Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(250);

            Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(250);

            Property(t => t.ShortDescription)
               .IsRequired().HasMaxLength(400);

            Property(t => t.QuizCategoryId)
                .IsRequired();

            Property(t => t.MetaTitle)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.MetaDescription)
                .IsOptional()
                .HasMaxLength(500);
        }
    }
}