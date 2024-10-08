#nullable disable

namespace Shared.Lib.Attributes;

public class SettingPropertyDefaultValueAttribute(object value) : Attribute
{
    public object Value { get; set; } = value;
}