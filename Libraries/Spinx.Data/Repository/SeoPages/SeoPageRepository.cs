using Spinx.Data.Infrastructure;
using Spinx.Domain.SeoPages;

namespace Spinx.Data.Repository.SeoPages
{
    public interface ISeoPageRepository : IRepository<SeoPage>
    {
        
    }

    public class SeoPageRepository : Repository<SeoPage>, ISeoPageRepository
    {
        public SeoPageRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}