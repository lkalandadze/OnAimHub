#nullable disable

namespace Shared.Domain.Entities;

public class Setting : BaseEntity<int>
{
    public Setting(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; }
    public string Value { get; set; }

    public void ChangeDetails(string value)
    {
        Value = value;
    }
}