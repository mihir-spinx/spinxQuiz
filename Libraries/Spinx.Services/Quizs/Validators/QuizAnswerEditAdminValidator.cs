using FluentValidation;
using Spinx.Services.QuizAnswers.DTOs;

namespace Spinx.Services.QuizAnswers.Validators
{
    public class QuizAnswerEditAdminValidator : AbstractValidator<QuizAnswerEditAdminDto>
    {
        public QuizAnswerEditAdminValidator()
        {
            RuleFor(v => v.Answer).NotEmpty().MaximumLength(500);
        }      
    }
}