using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IAppSettingsService
{
    string GetSetting(string key);
    void SetSetting(string key, string value);
    List<AppSetting> GetSettings();
    Task<bool> SetTwoFactorAuth(bool twoFactorAuth);
}
