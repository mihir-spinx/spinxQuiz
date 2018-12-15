using FluentValidation;
using Spinx.Services.QuizAnswers.DTOs;

namespace Spinx.Services.QuizAnswers.Validators
{
    public class QuizAnswerCreateAdminValidator : AbstractValidator<QuizAnswerCreateAdminDto>
    {
        public QuizAnswerCreateAdminValidator()
        {
            RuleFor(v => v.QuizQuestionId).NotEmpty();
            RuleFor(v => v.Answer).NotEmpty().MaximumLength(500);
        }
    }
}