using Spinx.Core.Extensions;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.QuizAnswers.Actions
{
    public class QuizAnswerAdminActionFactory
    {
        private readonly QuizAnswerAdminDeleteAction _deleteAction;

        public QuizAnswerAdminActionFactory(
           QuizAnswerAdminDeleteAction deleteAction)
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