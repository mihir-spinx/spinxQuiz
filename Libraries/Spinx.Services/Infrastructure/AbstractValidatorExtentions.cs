using FluentValidation;
using Spinx.Core;

namespace Spinx.Services.Infrastructure
{
    public static class AbstractValidatorExtentions
    {
        public static Result ValidateResult<TInstance>(this AbstractValidator<TInstance> validator, TInstance instance)
        {
            var results = validator.Validate(instance);

            if (results.IsValid) return new Result().SetSuccess();

            var result = new Result { Success = false };

            foreach (var validationFailure in results.Errors)
                result.Errors.Add(validationFailure.PropertyName, validationFailure.ErrorMessage);

            return result;
        }
    }
}