using Omu.ValueInjecter;
using Spinx.Domain.GeneralSettings;
using Spinx.Services.GeneralSettings.DTOs;

namespace Spinx.Services.GeneralSettings.Mappers
{
    public static class GeneralSettingAdminMapper
    {
        public static void Init()
        {
            Mapper.AddMap<GeneralSettingCreateAdminDto, GeneralSetting>((from, to) =>
            {
                var existing = to as GeneralSetting ?? new GeneralSetting();
                existing.InjectFrom(from);
                return existing;
            });

            Mapper.AddMap<GeneralSettingEditAdminDto, GeneralSetting>((from, to) =>
            {
                var existing = to as GeneralSetting ?? new GeneralSetting();
                existing.InjectFrom(from);
                return existing;
            });
        }
    }
}
