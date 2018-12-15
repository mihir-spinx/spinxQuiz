using FluentValidation;
using Spinx.Services.QuizQuestions.DTOs;

namespace Spinx.Services.QuizQuestions.Validators
{
    public class QuizQuestionEditAdminValidator : AbstractValidator<QuizQuestionEditAdminDto>
    {
        public QuizQuestionEditAdminValidator()
        {
            RuleFor(v => v.QuizId).NotEmpty();
            RuleFor(v => v.Question).NotEmpty().MaximumLength(240);
        }      
    }
}