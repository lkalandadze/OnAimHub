
namespace OnAim.Admin.Domain.HubEntities;

public class Setting : BaseEntity<int>
{
    public string SettingName { get; set; }
    public string Value { get; set; }

    public void Update(string value)
    {
        Value = value;
    }
}