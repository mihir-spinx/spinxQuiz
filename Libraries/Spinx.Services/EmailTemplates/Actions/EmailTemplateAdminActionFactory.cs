using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.EmailTemplates.Actions
{
    public class EmailTemplateAdminActionFactory
    {
        private readonly EmailTemplateAdminDeleteAction _deleteAction;

        public EmailTemplateAdminActionFactory(
            EmailTemplateAdminDeleteAction deleteAction)
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
