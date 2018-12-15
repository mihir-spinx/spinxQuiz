using System;

namespace Spinx.Web.Core.AppSettings
{
    public interface ISettingValueConverter
    {
        bool CanConvert(Type type);

        object Convert(string fromValue, Type convertToType);
    }
}