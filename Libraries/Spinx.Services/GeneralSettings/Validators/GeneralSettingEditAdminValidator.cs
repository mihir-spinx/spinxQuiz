using FluentValidation;
using Spinx.Data.Repository.GeneralSettings;
using Spinx.Services.GeneralSettings.DTOs;
using System.Linq;

namespace Spinx.Services.GeneralSettings.Validators
{
    public class GeneralSettingEditAdminValidator : AbstractValidator<GeneralSettingEditAdminDto>
    {

        public GeneralSettingEditAdminValidator()
        {
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.Value).NotEmpty().MaximumLength(100);
        }
       
    }
}