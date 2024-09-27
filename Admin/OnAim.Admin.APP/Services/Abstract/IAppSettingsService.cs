namespace OnAim.Admin.APP.Services.Abstract;

public interface IAppSettingsService
{
    string GetSetting(string key);
    void SetSetting(string key, string value);
}
