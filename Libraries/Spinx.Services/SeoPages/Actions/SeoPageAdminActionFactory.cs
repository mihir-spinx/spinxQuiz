using Spinx.Core.Extensions;
using Spinx.Services.ContactUsInquiries.Actions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.SeoPages.Actions
{
    public class SeoPageAdminActionFactory
    {
        private readonly SeoPageAdminDeleteAction _deleteAction;

        public SeoPageAdminActionFactory(
            SeoPageAdminDeleteAction deleteAction)
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
