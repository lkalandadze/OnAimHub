using System.Reflection;

namespace Shared.Lib.Helpers;

public static class ReflectionHelper
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

    public static void ReplacePropertyValuesDynamic<T>(object targetObject, string targetPropertyName, Func<T, T> transformFunction)
    {
        if (targetObject == null) return;

        var properties = targetObject.GetType().GetProperties();

        foreach (var property in properties)
        {
            try
            {
                // Skip indexed properties
                if (property.GetIndexParameters().Length > 0)
                {
                    Console.WriteLine($"Skipping indexed property: {property.Name}");
                    continue;
                }

                // Check if the property is a collection
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType) &&
                    property.PropertyType != typeof(string))
                {
                    var collection = property.GetValue(targetObject) as System.Collections.IEnumerable;
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            ReplacePropertyValuesDynamic(item, targetPropertyName, transformFunction); // Recurse into collection elements
                        }
                    }
                }
                else if (property.Name == targetPropertyName && property.PropertyType == typeof(T))
                {
                    // Process the target property
                    var currentValue = (T)property.GetValue(targetObject);
                    if (currentValue != null)
                    {
                        property.SetValue(targetObject, transformFunction(currentValue));
                    }
                }
                else
                {
                    // Recurse into other complex objects
                    var nestedObject = property.GetValue(targetObject);
                    if (nestedObject != null && !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                    {
                        ReplacePropertyValuesDynamic(nestedObject, targetPropertyName, transformFunction);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing property '{property.Name}': {ex.Message}");
                throw;
            }
        }
    }
}