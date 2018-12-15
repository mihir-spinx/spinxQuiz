using Spinx.Data.Infrastructure;
using Spinx.Domain.EmailTemplates;

namespace Spinx.Data.Repository.EmailTemplates
{
    public interface IEmailTemplateRepository : IRepository<EmailTemplate>
    {

    }

    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}