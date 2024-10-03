using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Domain.Entities;

public class AppSetting : BaseEntity
{
    public AppSetting(string key, string value)
    {
        Key = key;
        Value = value;
        DateCreated = SystemDate.Now;
        IsActive = true;
    }

    public string Key { get; set; }
    public string Value { get; set; }

}
