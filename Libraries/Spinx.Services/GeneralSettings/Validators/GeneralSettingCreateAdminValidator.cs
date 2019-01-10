using FluentValidation;
using System.Linq;
using Spinx.Data.Repository.GeneralSettings;
using Spinx.Services.GeneralSettings.DTOs;

namespace Spinx.Services.GeneralSettings.Validators
{
    public class GeneralSettingCreateAdminValidator : AbstractValidator<GeneralSettingCreateAdminDto>
    {
        private readonly IGeneralSettingRepository _GeneralSettingRepository;

        public GeneralSettingCreateAdminValidator(
            IGeneralSettingRepository GeneralSettingRepository)
        {
            _GeneralSettingRepository = GeneralSettingRepository;
            RuleFor(v => v.Slug).NotEmpty().MaximumLength(150).Must(UniqueName).WithMessage("GeneralSetting with this name already exists");
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Value).NotEmpty().MaximumLength(100);
        }

        private bool UniqueName(GeneralSettingCreateAdminDto dto, string slug)
        {
            return !_GeneralSettingRepository.AsNoTracking.Any(w => w.Slug == slug);
        }        
    }
}