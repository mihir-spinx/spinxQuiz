using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.ContactUsInquiries.Actions
{
    public class ContactUsInquiryAdminActionFactory
    {
        private readonly ContactUsInquiryAdminDeleteAction _deleteAction;

        public ContactUsInquiryAdminActionFactory(
            ContactUsInquiryAdminDeleteAction deleteAction)
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
