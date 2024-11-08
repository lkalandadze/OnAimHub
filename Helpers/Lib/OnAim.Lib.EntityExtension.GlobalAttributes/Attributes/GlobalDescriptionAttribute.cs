namespace OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
public class GlobalDescriptionAttribute : Attribute
{
    public GlobalDescriptionAttribute(string description)
    {
        Description = description;
    }
    public string Description { get; }

}