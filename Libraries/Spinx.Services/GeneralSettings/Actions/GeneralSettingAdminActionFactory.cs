using Spinx.Core.Extensions;
using Spinx.Services.ContactUsInquiries.Actions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.GeneralSettings.Actions
{
    public class GeneralSettingAdminActionFactory
    {
        private readonly GeneralSettingAdminDeleteAction _deleteAction;

        public GeneralSettingAdminActionFactory(
            GeneralSettingAdminDeleteAction deleteAction)
        {
            _deleteAction = deleteAction;
        }

        public BaseAction Action(string action)
        {
            if (action.NullSafeToLower() == "delete")
                return _deleteAction;

            return null;
        }
    }
}
