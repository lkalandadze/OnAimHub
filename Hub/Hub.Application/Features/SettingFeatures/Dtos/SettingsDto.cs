using Hub.Domain.Entities;

namespace Hub.Application.Features.SettingFeatures.Dtos;

public class SettingsDto
{
    public int Id { get; set; }
    public string SettingName { get; set; }
    public string Value { get; set; }

    public static SettingsDto MapFrom(HubSetting setting)
    {
        return new SettingsDto
        {
            Id = setting.Id,
            SettingName = setting.Name,
            Value = setting.Value,
        };
    }
}