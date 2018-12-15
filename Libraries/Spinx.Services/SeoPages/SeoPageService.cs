using Spinx.Data.Repository.SeoPages;
using Spinx.Domain.SeoPages;
using System.Linq;

namespace Spinx.Services.SeoPages
{
    public interface ISeoPageService
    {
        SeoPage GetPageMeta(string pageName);
    }

    public class SeoPageService : ISeoPageService
    {
        private readonly ISeoPageRepository _seoPageRepository;

        public SeoPageService(ISeoPageRepository seoPageRepository)
        {
            _seoPageRepository = seoPageRepository;
        }

        public SeoPage GetPageMeta(string pageName)
        {
            return _seoPageRepository.AsNoTracking.FirstOrDefault(w => w.Name == pageName);
        }
    }
}