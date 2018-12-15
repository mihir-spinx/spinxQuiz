using ImpromptuInterface;
using Spinx.Web.Core.AppSettings.ValueConverters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Spinx.Web.Core.AppSettings
{
    public abstract class AppSettings : DynamicObject
    {
        private readonly IEnumerable<ISettingValueConverter> _defaultValueConverters = new ISettingValueConverter[]
        {
            new DateTimeValueConverter(), 
            new EnumValueConverter(),
            new ScalarValueConverter(),
            new CommaDelimitedScalarValueConverter(),
            new CommaDelimitedConstructorArgumentValueConverter() 
        };

        public IEnumerable<ISettingValueConverter> DefaultValueConverters
        {
            get { return _defaultValueConverters; }
        }
    }

    public class AppSettings<TSettings> : AppSettings where TSettings : class
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly ISettingsRepository _settingsRepository;
        private readonly List<ISettingValueConverter> _settingValueConverters;

        public AppSettings()
            : this(new WebConfigAppSettingsRepository())
        {
        }

        public AppSettings(ISettingsRepository settingsRepository)
            : this(settingsRepository, null)
        {
        }

        public AppSettings(IEnumerable<ISettingValueConverter> settingValueConverters)
            : this(new WebConfigAppSettingsRepository(), settingValueConverters)
        {
        }

        public AppSettings(ISettingsRepository settingsRepository,
            IEnumerable<ISettingValueConverter> settingValueConverters)
        {
            _settingsRepository = settingsRepository;

            _settingValueConverters = new List<ISettingValueConverter>(settingValueConverters ?? Enumerable.Empty<ISettingValueConverter>());

            _settingValueConverters.AddRange(DefaultValueConverters);
        }

        public TSettings Settings
        {
            get
            {
                return this.ActLike<TSettings>();
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;

            var propInfo = typeof (TSettings).GetProperty(name);

            if (propInfo == null)
                return base.TryGetMember(binder, out result);

            if (_cache.ContainsKey(name))
            {
                result = _cache[name];
                return true;
            }

            var fromValue = _settingsRepository.Get(name);

            var converter = _settingValueConverters.FirstOrDefault(a => a.CanConvert(propInfo.PropertyType));

            if (converter == null)
                throw new InvalidOperationException(
                    $"No Value Converter found for setting type {propInfo.PropertyType.Name}");

            result = converter.Convert(fromValue, propInfo.PropertyType);

            _cache.Add(name, result);

            return true;
        }
    }
}

