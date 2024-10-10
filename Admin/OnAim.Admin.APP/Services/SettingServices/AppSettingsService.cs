using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services.SettingServices;

public class AppSettingsService : IAppSettingsService
{
    private readonly IRepository<AppSetting> _appSettingRepository;
    private readonly IRepository<OnAim.Admin.Domain.Entities.User> _repository;

    public AppSettingsService(
        IRepository<AppSetting> appSettingRepository,
        IRepository<OnAim.Admin.Domain.Entities.User> repository
        )
    {
        _appSettingRepository = appSettingRepository;
        _repository = repository;
    }

    public string GetSetting(string key)
    {
        var setting = _appSettingRepository.Query(s => s.Key == key).SingleOrDefault();
        var value = setting?.Value;
        return value;
    }

    public List<AppSetting> GetSettings()
    {
        var setting = _appSettingRepository.Query();
        var value = setting;
        return value.ToList();
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

    public async Task<bool> SetTwoFactorAuth(bool twoFactorAuth)
    {
        var settingValue = twoFactorAuth ? "true" : "false";
        SetSetting("TwoFactorAuthEnabled", settingValue);

        var users = await _repository.Query().ToListAsync();

        foreach (var user in users)
        {
            user.IsTwoFactorEnabled = twoFactorAuth;
            await _repository.CommitChanges();
        }

        return true;
    }
}
