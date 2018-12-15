using FluentValidation;
using Spinx.Services.QuizCategories.DTOs;

namespace Spinx.Services.QuizCategories.Validators
{
    public class QuizCategoryEditAdminValidator : AbstractValidator<QuizCategoryEditAdminDto>
    {
        public QuizCategoryEditAdminValidator()
        {
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.CategoryIcon).NotEmpty();
        }      
    }
}