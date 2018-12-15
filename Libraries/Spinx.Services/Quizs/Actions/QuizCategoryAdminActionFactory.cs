using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.QuizCategories.Actions
{
    public class QuizCategoryAdminActionFactory
    {
        private readonly QuizCategoryAdminDeleteAction _deleteAction;
        private readonly QuizCategoryAdminInactiveAction _inactiveAction;
        private readonly QuizCategoryAdminActiveAction _activeAction;

        public QuizCategoryAdminActionFactory(
           QuizCategoryAdminActiveAction activeAction,
           QuizCategoryAdminInactiveAction inactiveAction,
           QuizCategoryAdminDeleteAction deleteAction)
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