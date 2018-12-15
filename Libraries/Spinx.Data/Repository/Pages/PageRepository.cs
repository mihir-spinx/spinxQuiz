using Spinx.Data.Infrastructure;
using Spinx.Domain.Pages;

namespace Spinx.Data.Repository.Pages
{
    public interface IPageRepository : IRepository<Page>
    {
        
    }

    public class PageRepository : Repository<Page>, IPageRepository
    {
        public PageRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}