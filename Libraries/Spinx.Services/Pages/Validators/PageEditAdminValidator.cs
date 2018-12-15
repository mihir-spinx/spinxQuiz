using FluentValidation;
using Spinx.Data.Repository.Pages;
using Spinx.Services.Content;
using Spinx.Services.Pages.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.Pages.Validators
{
    public class PageEditAdminValidator : AbstractValidator<PageEditAdminDto>
    {
        private readonly IPageRepository _pageRepository;
       

        public PageEditAdminValidator(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;

            RuleFor(v => v.Title).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Slug).NotEmpty().MaximumLength(100)
                .Must(UniqueSlug).WithMessage(Messages.UniqueRecord);
            RuleFor(v => v.MetaTitle).MaximumLength(70);
            RuleFor(v => v.MetaDescription).MaximumLength(300);
        }

        private bool UniqueSlug(PageEditAdminDto dto, string slug)
        {
            return !_pageRepository.AsNoTracking.Any(w =>w.Slug == slug && w.Id != dto.Id);
        }

       
    }
}
