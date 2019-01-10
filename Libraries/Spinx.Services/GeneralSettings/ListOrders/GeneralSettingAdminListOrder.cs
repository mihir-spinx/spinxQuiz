using Spinx.Domain.GeneralSettings;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.GeneralSettings.ListOrders
{
    public class GeneralSettingAdminListOrder : BaseListOrder<GeneralSetting>
    {
        public GeneralSettingAdminListOrder(IQueryable<GeneralSetting> query, BaseFilterDto dto) : base (query, dto) { }

        internal void Name()
        {
            Query = OrderBy(t => t.Name);
        }
        internal void Value()
        {
            Query = OrderBy(t => t.Value);
        }
    }
}