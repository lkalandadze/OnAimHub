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

                if (settingsFromDb.TryGetValue(settingName, out var dbValue))
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
                        propertyInstance.SetValue(defaultValue);
                    }
                }
            }
        }
    }

    public void UpdateSetting(string propertyName, object value)
    {
        var property = this.GetType().GetProperty(propertyName);

        if (property?.GetValue(this) is SettingProperty settingProperty)
        {
            SetValue(settingProperty, propertyName, value);
        }
    }

    public void SetValue(SettingProperty property, string name, object value)
    {
        _settingRepository.UpdateValue(name, value);
        property.SetValue(value);
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