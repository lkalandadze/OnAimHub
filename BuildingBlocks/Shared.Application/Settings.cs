#nullable disable

using Shared.Domain.Abstractions.Repository;
using Shared.Lib.Attributes;
using System.Reflection;

namespace Shared.Application;

public abstract class Settings
{
    private readonly ISettingRepository _settingRepository;

    protected Settings(ISettingRepository settingRepository)
    {
        _settingRepository = settingRepository;
        Initialize();
    }

    private void Initialize()
    {
        var settingsFromDb = _settingRepository.GetSettings();

        foreach (var prop in this.GetType().GetProperties())
        {
            if (prop.PropertyType.IsSubclassOf(typeof(SettingProperty)))
            {
                var settingProperty = Activator.CreateInstance(prop.PropertyType);
                prop.SetValue(this, settingProperty);

                var settingName = prop.Name;
                object dbValue;

                if (settingsFromDb.TryGetValue(settingName, out dbValue))
                {
                    if (settingProperty is SettingProperty propertyInstance)
                    {
                        propertyInstance.SetValue(dbValue);
                    }
                }
                else
                {
                    var defaultValue = prop.GetCustomAttribute<SettingPropertyDefaultValueAttribute>()?.Value;

                    if (defaultValue != null && settingProperty is SettingProperty propertyInstance)
                    {
                        _settingRepository.GetOrCreateValue(settingName, defaultValue);
                        propertyInstance.SetValue(defaultValue);
                    }
                }
            }
        }
    }

    public void UpdateSetting(string propertyName, object value)
    {
        var property = GetType().GetProperty(propertyName);

        if (property?.GetValue(this) is SettingProperty settingProperty)
        {
            SetValue(propertyName, value);
        }
    }

    public void SetValue(string name, object value)
    {
        var property = GetType().GetProperties().First(x => x.Name == name);
        _settingRepository.UpdateValue(name, value);
        (property.GetValue(this) as SettingProperty).SetValue(value);
    }
}

public abstract class SettingProperty
{
    public abstract void SetValue(object newValue);
}

public class SettingProperty<T> : SettingProperty
{
    private T _value;

    public T Value
    {
        get
        {
            return _value;
        }
    }

    public override void SetValue(object newValue) 
    {
        _value = (T)Convert.ChangeType(newValue, typeof(T));
    }
}