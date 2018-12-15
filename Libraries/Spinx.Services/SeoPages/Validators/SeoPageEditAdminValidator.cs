using FluentValidation;
using Spinx.Data.Repository.SeoPages;
using Spinx.Services.SeoPages.DTOs;
using System.Linq;

namespace Spinx.Services.SeoPages.Validators
{
    public class SeoPageEditAdminValidator : AbstractValidator<SeoPageEditAdminDto>
    {
        private readonly ISeoPageRepository _seoPageRepository;        

        public SeoPageEditAdminValidator(
            ISeoPageRepository seoPageRepository)
        {
            _seoPageRepository = seoPageRepository;
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100).Must(UniqueName).WithMessage("Seo Page with this name already exists");
            RuleFor(v => v.MetaTitle).MaximumLength(70);
            RuleFor(v => v.MetaDescription).MaximumLength(300);
        }

        private bool UniqueName(SeoPageEditAdminDto dto, string name)
        {
            return !_seoPageRepository.AsNoTracking.Any(w => w.Id != dto.Id  && w.Name == name);
        }
    }
}