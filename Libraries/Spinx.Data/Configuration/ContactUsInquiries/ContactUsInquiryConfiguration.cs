using Spinx.Domain.ContactUsInquiries;
using System.Data.Entity.ModelConfiguration;

namespace Spinx.Data.Configuration.ContactUsInquiries
{
    public class ContactUsInquiryConfiguration : EntityTypeConfiguration<ContactUsInquiry>
    {
        public ContactUsInquiryConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(254);

            Property(t => t.Phone)
                .HasMaxLength(20);

            Property(t => t.Details)
                .IsRequired();
        }
    }
}