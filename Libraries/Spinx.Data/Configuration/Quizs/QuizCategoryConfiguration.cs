using Spinx.Domain.QuizCategories;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.QuizCategories
{
    public class QuizCategoryConfiguration : EntityTypeConfiguration<QuizCategory>
    {
        public QuizCategoryConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.CategoryIcon)
               .IsRequired()
               .HasMaxLength(250);
        }
    }
}