using System.Reflection;

namespace Shared.Lib.Helpers;

public static class ObjectHelper
{
    public static void SetProperty(object obj, PropertyInfo property, object value)
    {
        var type = obj.GetType();

        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, value);
        }
    }

    public static void SetProperty(object obj, string propertyName, object value)
    {
        var type = obj.GetType();
        var property = type.GetProperty(propertyName);

        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, value);
        }
    }
}