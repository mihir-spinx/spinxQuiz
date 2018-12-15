using Omu.ValueInjecter;
using Spinx.Domain.SeoPages;
using Spinx.Services.SeoPages.DTOs;

namespace Spinx.Services.SeoPages.Mappers
{
    public static class SeoPageAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<SeoPageCreateAdminDto, SeoPage>((from, to) =>
            {
                var existing = to as SeoPage ?? new SeoPage();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<SeoPageEditAdminDto, SeoPage>((from, to) =>
            {
                var existing = to as SeoPage ?? new SeoPage();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
