using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.QuizQuestions.Actions
{
    public class QuizQuestionAdminActionFactory
    {
        private readonly QuizQuestionAdminDeleteAction _deleteAction;
        private readonly QuizQuestionAdminInactiveAction _inactiveAction;
        private readonly QuizQuestionAdminActiveAction _activeAction;

        public QuizQuestionAdminActionFactory(
           QuizQuestionAdminActiveAction activeAction,
           QuizQuestionAdminInactiveAction inactiveAction,
           QuizQuestionAdminDeleteAction deleteAction)
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