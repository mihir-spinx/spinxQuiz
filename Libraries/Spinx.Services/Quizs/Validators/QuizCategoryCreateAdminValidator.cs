using FluentValidation;
using Spinx.Services.QuizCategories.DTOs;

namespace Spinx.Services.QuizCategories.Validators
{
    public class QuizCategoryCreateAdminValidator : AbstractValidator<QuizCategoryCreateAdminDto>
    {
        public QuizCategoryCreateAdminValidator()
        {
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.CategoryIcon).NotEmpty();
        }    
    }
}