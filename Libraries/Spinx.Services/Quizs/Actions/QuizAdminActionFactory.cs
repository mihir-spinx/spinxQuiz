using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.Quizs.Actions
{
    public class QuizAdminActionFactory
    {
        private readonly QuizAdminDeleteAction _deleteAction;
        private readonly QuizAdminInactiveAction _inactiveAction;
        private readonly QuizAdminActiveAction _activeAction;

        public QuizAdminActionFactory(
           QuizAdminActiveAction activeAction,
           QuizAdminInactiveAction inactiveAction,
           QuizAdminDeleteAction deleteAction)
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