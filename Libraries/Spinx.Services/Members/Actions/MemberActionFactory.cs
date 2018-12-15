using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.Members.Actions
{
    public class MemberActionFactory
    {
        private readonly MemberDeleteAction _deleteAction;
        private readonly MemberInactiveAction _inactiveAction;
        private readonly MemberActiveAction _activeAction;

        public MemberActionFactory(
                   MemberActiveAction activeAction,
                   MemberInactiveAction inactiveAction,
                   MemberDeleteAction deleteAction)
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

            return action.NullSafeToLower() == "delete" ? _deleteAction : null;
        }
    }
}
