using FluentValidation;
using Spinx.Data.Repository.Quizs;
using Spinx.Services.Quizs.DTOs;
using System.Linq;

namespace Spinx.Services.Quizs.Validators
{
    public class QuizEditAdminValidator : AbstractValidator<QuizEditAdminDto>
    {
        private readonly IQuizRepository _quizRepository;        

        public QuizEditAdminValidator(
            IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
            RuleFor(v => v.Title).NotEmpty().MaximumLength(100);
            RuleFor(v => v.ShortDescription).NotEmpty().WithMessage("Special Instruction can not be empty").MaximumLength(400);
            RuleFor(v => v.QuizCategoryId).NotEmpty().WithMessage("Category can not be empty.");
            RuleFor(v => v.MetaTitle).MaximumLength(70);
            RuleFor(v => v.MetaDescription).MaximumLength(300);
        }

        private bool UniqueSlug(QuizEditAdminDto dto, string slug)
        {
            return !_quizRepository.AsNoTracking.Any(w => w.Id != dto.Id  && w.Slug == slug);
        }
    }
}