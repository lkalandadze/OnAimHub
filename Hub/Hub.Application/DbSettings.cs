using Hub.Domain.Absractions;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using System.Reflection;

namespace Hub.Application
{
    public class DbSettings
    {
        [SettingProperty(50)]
        public int MaxReferralsPerUser { get; set; }

        [SettingProperty(nameof(Currency.FreeSpin))]
        public string ReferrerPrizeCurrencyId { get; set; }

        [SettingProperty(nameof(Currency.FreeSpin))]
        public string ReferralPrizeCurrencyId { get; set; }

        [SettingProperty(100)]
        public int ReferrerPrizeAmount { get; set; }

        [SettingProperty(50)]
        public int ReferralPrizeAmount { get; set; }

        private static DbSettings? _instance;

        public static DbSettings Instance
        {
            get
            {
                if (_instance is not null)
                {
                    return _instance;
                }

                _instance = new DbSettings();

                return _instance;
            }
        }

        public static void Init(ISettingRepository settingRepository)
        {
            PropertyInfo[] properties = Instance.GetType().GetProperties();

            var settings = settingRepository.GetAllSettings();

            foreach (var property in properties.Where(x => x.GetCustomAttributes(typeof(SettingPropertyAttribute), true).Any()))
            {
                var propertyName = property.Name;

                SettingPropertyAttribute? attribute = property.GetCustomAttributes(typeof(SettingPropertyAttribute), true).FirstOrDefault() as SettingPropertyAttribute;

                if (attribute == null)
                {
                    continue;
                }

                var dbProperty = settings.FirstOrDefault(x => x.SettingName.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

                if (dbProperty == null)
                {
                    property.SetValue(Instance, attribute?.DefaultValue);
                    settingRepository.InsertAsync(new Setting { SettingName = propertyName, Value = attribute?.DefaultValue?.ToString() });
                }
                else
                {
                    if (property.PropertyType.IsEnum)
                    {
                        property.SetValue(Instance, Enum.Parse(property.PropertyType, dbProperty.Value));
                    }
                    else
                    {
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                            if (dbProperty.Value != null)
                            {
                                property.SetValue(Instance, Convert.ChangeType(dbProperty.Value, underlyingType));
                            }
                            else
                            {
                                property.SetValue(Instance, null);
                            }
                        }
                        else
                        {
                            property.SetValue(Instance, Convert.ChangeType(dbProperty.Value, property.PropertyType));
                        }
                    }
                }
            }

            settingRepository.SaveChanges();
        }
    }

    class SettingPropertyAttribute : Attribute
    {
        public object? DefaultValue { get; set; }
        public SettingPropertyAttribute(object? defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}
