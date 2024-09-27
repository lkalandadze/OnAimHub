using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services.SettingServices;

public class AppSettingsService : IAppSettingsService
{
    private readonly IRepository<AppSetting> _appSettingRepository;
    private readonly ILogger<AppSettingsService> _logger;

    public AppSettingsService(IRepository<AppSetting> appSettingRepository, ILogger<AppSettingsService> logger)
    {
        _appSettingRepository = appSettingRepository;
        _logger = logger;
    }

    public string GetSetting(string key)
    {
        var setting = _appSettingRepository.Query(s => s.Key == key).SingleOrDefault();
        var value = setting?.Value;
        _logger.LogInformation($"Getting setting: {key} = {value}");
        return value;
    }

    public void SetSetting(string key, string value)
    {
        var setting = _appSettingRepository.Query(s => s.Key == key).SingleOrDefault();
        if (setting != null)
        {
            setting.Value = value;
            _appSettingRepository.CommitChanges();
            _logger.LogInformation($"Updated setting: {key} = {value}");
        }
        else
        {
            _appSettingRepository.Store(new AppSetting { Key = key, Value = value });
            _logger.LogInformation($"Added setting: {key} = {value}");
        }
        _appSettingRepository.CommitChanges();
    }
}
