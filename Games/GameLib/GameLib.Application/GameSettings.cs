#nullable disable

using GameLib.Application.Managers;
using Shared.Application;
using Shared.Lib.Attributes;

namespace GameLib.Application;

public class GameSettings : Settings
{
    public GameSettings() : base(RepositoryManager.GameSettingRepository())
    {
    }

    [SettingPropertyDefaultValue(true)]
    public SettingProperty<bool> IsActive { get; set; }

    [SettingPropertyDefaultValue("TODO: description of wheel")]
    public SettingProperty<string> Description { get; set; }
}