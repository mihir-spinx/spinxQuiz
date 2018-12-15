using FluentValidation;
using System.Linq;
using Spinx.Data.Repository.SeoPages;
using Spinx.Services.SeoPages.DTOs;

namespace Spinx.Services.SeoPages.Validators
{
    public class SeoPageCreateAdminValidator : AbstractValidator<SeoPageCreateAdminDto>
    {
        private readonly ISeoPageRepository _seoPageRepository;

        public SeoPageCreateAdminValidator(
            ISeoPageRepository seoPageRepository)
        {
            _seoPageRepository = seoPageRepository;
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100).Must(UniqueName).WithMessage("Seo Page with this name already exists");
            RuleFor(v => v.MetaTitle).MaximumLength(70);
            RuleFor(v => v.MetaDescription).MaximumLength(300);
        }

        private bool UniqueName(SeoPageCreateAdminDto dto, string name)
        {
            return !_seoPageRepository.AsNoTracking.Any(w => w.Name == name);
        }        
    }
}