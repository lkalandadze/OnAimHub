using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class HubSettingRepository(HubDbContext context) : BaseRepository<HubDbContext, HubSetting>(context), IHubSettingRepository
{
    public Dictionary<string, object> GetSettings()
    {
        return _context.HubSettings.ToDictionary(setting => setting.Name, setting => (object)setting.Value);
    }

    public object GetOrCreateValue(string dbSettingPropertyName, object defaultValue)
    {
        var setting = _context.HubSettings.FirstOrDefault(s => s.Name == dbSettingPropertyName);

        if (setting == null)
        {
            setting = new HubSetting(dbSettingPropertyName, defaultValue?.ToString());
            _context.HubSettings.Add(setting);
            _context.SaveChanges();
        }

        return setting.Value;
    }

    public void UpdateValue<T>(string nameOfProperty, T value)
    {
        var setting = _context.HubSettings.FirstOrDefault(s => s.Name == nameOfProperty);

        if (setting != null && setting.Value != value?.ToString())
        {
            setting.ChangeDetails(value?.ToString());
            _context.SaveChanges();
        }
    }
}