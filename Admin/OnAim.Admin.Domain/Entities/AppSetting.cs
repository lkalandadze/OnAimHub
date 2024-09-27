using OnAim.Admin.Domain.Entities.Abstract;

namespace OnAim.Admin.Domain.Entities;

public class AppSetting : BaseEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
}
