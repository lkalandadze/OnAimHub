#nullable disable

namespace Shared.Domain.Abstractions.Repository;

public interface ISettingRepository
{
    object GetOrCreateValue(string dbSettingPropertyName, object defaultValue);
    void UpdateValue<T>(string nameOfProperty, T value);
}