using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Shared.Domain.Abstractions.Repository;

namespace GameLib.Infrastructure.Repositories;

public class GameSettingRepository<T> : ISettingRepository where T : GameConfiguration<T>
{
    private readonly SharedGameConfigDbContext<T> _context;

    public GameSettingRepository(SharedGameConfigDbContext<T> context)
    {
        _context = context;
    }

    public object GetOrCreateValue(string dbSettingPropertyName, object defaultValue)
    {
        var setting = _context.GameSettings.FirstOrDefault(s => s.Name == dbSettingPropertyName);

        if (setting == null)
        {
            setting = new GameSetting(dbSettingPropertyName, defaultValue?.ToString());
            _context.GameSettings.Add(setting);
            _context.SaveChanges();
        }

        return setting.Value;
    }

    public void UpdateValue<T>(string nameOfProperty, T value)
    {
        var setting = _context.GameSettings.FirstOrDefault(s => s.Name == nameOfProperty);

        if (setting != null && setting.Value != value?.ToString())
        {
            setting.ChangeDetails(value?.ToString());
            _context.SaveChanges();
        }
    }
}