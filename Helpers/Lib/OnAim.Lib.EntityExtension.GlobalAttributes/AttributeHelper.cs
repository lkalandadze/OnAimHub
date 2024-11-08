using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using System.Reflection;

namespace OnAim.Lib.EntityExtension.GlobalAttributes;

public class AttributeHelper
{
    public static bool IsHidden(MemberInfo member)
    {
        return Attribute.IsDefined(member, typeof(IgnoreIncludeAllAttribute));
    }
}
