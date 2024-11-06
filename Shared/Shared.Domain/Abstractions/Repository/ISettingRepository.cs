#nullable disable

using Shared.Domain.Entities;
using System.Linq.Expressions;

namespace Shared.Domain.Abstractions.Repository;

public interface ISettingRepository
{
    Dictionary<string, object> GetSettings();
    object GetOrCreateValue(string dbSettingPropertyName, object defaultValue);
    void UpdateValue<T>(string nameOfProperty, T value);
}