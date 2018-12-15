using Spinx.Data.Repository.Pages;
using Spinx.Domain.Pages;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Spinx.Services.Pages
{
    public interface IPageService
    {
        Page GetPageBySlug(string slug);
    }

    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(
            IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public Page GetPageBySlug(string slug)
        {
            return _pageRepository.AsNoTracking                                
                .Where(w => w.Slug == slug && w.IsActive)
                .DeferredFirstOrDefault()
                .FromCache("Pages");
        }
    }
}