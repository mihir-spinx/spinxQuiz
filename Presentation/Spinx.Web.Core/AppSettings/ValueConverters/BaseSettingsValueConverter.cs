using System;

namespace Spinx.Web.Core.AppSettings.ValueConverters
{
    public abstract class BaseSettingsValueConverter<TConvertTo> : ISettingValueConverter
    {
        public bool CanConvert(Type type)
        {
            return typeof (TConvertTo).IsAssignableFrom(type);
        }

        public object Convert(string fromValue, Type convertToType)
        {
            return Convert(fromValue);
        }

        public abstract TConvertTo Convert(string fromValue);
    }
}