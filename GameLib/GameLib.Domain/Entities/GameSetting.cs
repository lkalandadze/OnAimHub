#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class GameSetting : Setting
{
    public GameSetting(string name, string value) : base(name, value)
    {
    }
}