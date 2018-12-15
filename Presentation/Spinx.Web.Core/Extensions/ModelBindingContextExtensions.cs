using System.Web.Mvc;

namespace Spinx.Web.Core.Extensions
{
    public static class ModelBindingContextExtensions
    {
        public static ValueProviderResult GetValueFromValueProvider(this ModelBindingContext bindingContext, bool performRequestValidation)
        {
            return (bindingContext.ValueProvider is IUnvalidatedValueProvider unvalidatedValueProvider)
                ? unvalidatedValueProvider.GetValue(bindingContext.ModelName, !performRequestValidation)
                : bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        }
    }
}