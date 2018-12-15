using System;
using System.Linq;

namespace Spinx.Web.Core.AppSettings.ValueConverters
{
    public class ScalarValueConverter : ISettingValueConverter
    {
        public bool CanConvert(Type type)
        {
            return type.GetInterfaces().Any(i=>i == typeof(IConvertible));
        }

        public object Convert(string fromValue, Type convertToType)
        {
            return System.Convert.ChangeType(fromValue, convertToType);
        }
    }
}