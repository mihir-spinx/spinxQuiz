using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Spinx.Web.Core.Extensions;

namespace Spinx.Web.Core.ModelBinders
{
    public class CommaSeparatedValuesModelBinder : DefaultModelBinder
    {
        private static readonly MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod("ToArray");

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            // First check if request validation is required
            var shouldPerformRequestValidation = controllerContext.Controller.ValidateRequest && bindingContext.ModelMetadata.RequestValidationEnabled;

            // Get value
            var valueProviderResult = bindingContext.GetValueFromValueProvider(shouldPerformRequestValidation);

            if (valueProviderResult == null)
                return null;
            
            if (propertyDescriptor.PropertyType.GetInterface(typeof(IEnumerable).Name) != null)
            {
                //var actualValue = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);
                var attemptedValue = valueProviderResult.AttemptedValue;

                if (!string.IsNullOrWhiteSpace(attemptedValue ) && attemptedValue.Contains(","))
                {
                    var valueType = propertyDescriptor.PropertyType.GetElementType() ??
                                    propertyDescriptor.PropertyType.GetGenericArguments().FirstOrDefault();

                    if (valueType != null && valueType.GetInterface(typeof(IConvertible).Name) != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));

                        foreach (var splitValue in attemptedValue.Split(','))
                        {
                            if (valueType.IsEnum)
                            {
                                try
                                {
                                    list.Add(Enum.Parse(valueType, splitValue));
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                            else
                            {
                                list.Add(Convert.ChangeType(splitValue, valueType));
                            }
                        }

                        return propertyDescriptor.PropertyType.IsArray
                            ? ToArrayMethod.MakeGenericMethod(valueType).Invoke(this, new object[] { list })
                            : list;
                    }
                }
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
}