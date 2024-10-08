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

    protected void Initialize()
    {
        var settingsType = this.GetType();

        var settingProperties = settingsType.GetProperties().Where(prop =>
                           prop.PropertyType.GetGenericTypeDefinition() == typeof(SettingProperty<>)).ToList();

        foreach (var property in settingProperties)
        {
            var propertyValue = Activator.CreateInstance(property.PropertyType);
            property.SetValue(this, propertyValue);

            var defaultValue = property.GetCustomAttribute<SettingPropertyDefaultValueAttribute>()?.Value;

            _settingRepository.GetOrCreateValue(property.Name, defaultValue);
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