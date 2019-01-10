using Spinx.Services.Infrastructure;

namespace Spinx.Services.GeneralSettings.DTOs
{
    public class GeneralSettingAdminFilterDto: BaseFilterDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}