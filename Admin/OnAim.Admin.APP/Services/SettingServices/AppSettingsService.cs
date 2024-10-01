using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services.SettingServices;

public class AppSettingsService : IAppSettingsService
{
    private readonly IRepository<AppSetting> _appSettingRepository;

    public AppSettingsService(IRepository<AppSetting> appSettingRepository)
    {
        _appSettingRepository = appSettingRepository;
    }

    public string GetSetting(string key)
    {
        var setting = _appSettingRepository.Query(s => s.Key == key).SingleOrDefault();
        var value = setting?.Value;
        return value;
    }

    public void SetSetting(string key, string value)
    {
        var setting = _appSettingRepository.Query(s => s.Key == key).SingleOrDefault();
        if (setting != null)
        {
            setting.Value = value;
            _appSettingRepository.CommitChanges();
        }
        else
        {
            _appSettingRepository.Store(new AppSetting(key, value));
        }
        _appSettingRepository.CommitChanges();
    }
}
