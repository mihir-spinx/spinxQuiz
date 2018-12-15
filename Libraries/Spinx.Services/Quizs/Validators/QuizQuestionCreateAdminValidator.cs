using FluentValidation;
using Spinx.Services.QuizQuestions.DTOs;

namespace Spinx.Services.QuizQuestions.Validators
{
    public class QuizQuestionCreateAdminValidator : AbstractValidator<QuizQuestionCreateAdminDto>
    {
        public QuizQuestionCreateAdminValidator()
        {
            RuleFor(v => v.QuizId).NotEmpty();
            RuleFor(v => v.Question).NotEmpty().MaximumLength(240);
        }
    }
}