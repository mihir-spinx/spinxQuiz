using Omu.ValueInjecter;
using Spinx.Domain.Pages;
using Spinx.Services.Pages.DTOs;

namespace Spinx.Services.Pages.Mappers
{
    public static class PageAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<PageCreateAdminDto, Page>((from, to) =>
            {
                var existing = to as Page ?? new Page();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<PageEditAdminDto, Page>((from, to) =>
            {
                var existing = to as Page ?? new Page();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
