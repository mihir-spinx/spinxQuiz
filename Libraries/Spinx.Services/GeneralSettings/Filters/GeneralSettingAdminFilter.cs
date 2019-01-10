using Spinx.Domain.GeneralSettings;
using Spinx.Services.Infrastructure;
using System.Linq;
using Spinx.Services.GeneralSettings.DTOs;

namespace Spinx.Services.GeneralSettings.Filters
{
    public class GeneralSettingAdminFilter : BaseFilter<GeneralSetting, GeneralSettingAdminFilterDto>
    {
        public GeneralSettingAdminFilter(IQueryable<GeneralSetting> query, GeneralSettingAdminFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = Query.Where(w => w.Name.Contains(Dto.Name));
        }
        internal void Value()
        {
            Query = Query.Where(w => w.Value.Contains(Dto.Value));
        }
    }
}