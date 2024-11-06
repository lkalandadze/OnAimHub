using Shared.Domain.Entities;

namespace OnAim.Admin.Domain.Entities;

public class AppSetting : Setting
{
    public AppSetting(string name, string value) : base(name, value)
    {
    }
}