using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.Infrasturcture.Repositories;

public class AppSettingRepository : IAppSettingRepository
{
    private readonly DatabaseContext context;

    public AppSettingRepository(DatabaseContext databaseContext)
    {
        context = databaseContext;
    }
    public Dictionary<string, object> GetSettings()
        => context.AppSettings.ToDictionary(setting => setting.Name, setting => (object)setting.Value);

    public object GetOrCreateValue(string dbSettingPropertyName, object defaultValue)
    {
        var setting = context.AppSettings.FirstOrDefault(s => s.Name == dbSettingPropertyName);

        if (setting == null)
        {
            setting = new AppSetting(dbSettingPropertyName, defaultValue?.ToString());
            context.AppSettings.Add(setting);
            context.SaveChanges();
        }

        return setting.Value;
    }

    public void UpdateValue<T>(string nameOfProperty, T value)
    {
        var setting = context.AppSettings.FirstOrDefault(s => s.Name == nameOfProperty);

        if (setting != null && setting.Value != value?.ToString())
        {
            setting.ChangeDetails(value?.ToString());
            context.SaveChanges();
        }
    }
}
