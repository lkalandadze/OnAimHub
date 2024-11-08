namespace Shared.Lib.Helpers;

public static class TypeHelper
{
    public static bool IsGenericCollection(Type type)
    {
        return type.IsGenericType && (typeof(ICollection<>).IsAssignableFrom(type.GetGenericTypeDefinition()) || typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
    }

    public static bool In<T>(this T item, params T[] source)
    {
        return source.Contains(item);
    }

    public static string TypeName<T>(string value)
    {
        if (typeof(T) == typeof(string))
        {
            return "text";
        }
        else if (typeof(T) == typeof(int))
        {
            return "integer number";
        }
        if (typeof(T).In(typeof(double), typeof(decimal)))
        {
            return "number";
        }
        if (typeof(T) == typeof(bool))
        {
            return "TRUE or FALSE";
        }
        if (typeof(T) == typeof(DateTime))
        {
            return "date";
        }

        return typeof(T).Name;
    }
}