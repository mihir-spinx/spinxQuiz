using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.Pages.Actions
{
    public class PageAdminActionFactory
    {
        private readonly PageAdminActiveAction _activeAction;
        private readonly PageAdminInactiveAction _inactiveAction;
        private readonly PageAdminDeleteAction _deleteAction;

        public PageAdminActionFactory(
            PageAdminActiveAction activeAction,
            PageAdminInactiveAction inactiveAction,
            PageAdminDeleteAction deleteAction)
        {
            _activeAction = activeAction;
            _inactiveAction = inactiveAction;
            _deleteAction = deleteAction;
        }

        public BaseAction Action(string action)
        {
            if (action.NullSafeToLower() == "active")
                return _activeAction;

            if (action.NullSafeToLower() == "inactive")
                return _inactiveAction;
            
            if (action.NullSafeToLower() == "delete")
                return _deleteAction;

            return null;
        }
    }
}
