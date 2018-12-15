using Spinx.Domain.Pages;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.Pages
{
    public class PageConfiguration : EntityTypeConfiguration<Page>
    {
        public PageConfiguration()
        {
            Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.MetaTitle)
                .IsOptional()
                .HasMaxLength(100);

            Property(t => t.MetaDescription)
                .IsOptional()
                .HasMaxLength(500);
        }
    }
}