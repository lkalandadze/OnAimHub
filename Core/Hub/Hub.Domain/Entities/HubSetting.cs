#nullable disable
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class HubSetting : Setting
{
    public HubSetting(string name, string value) : base(name, value)
    {
    }
}