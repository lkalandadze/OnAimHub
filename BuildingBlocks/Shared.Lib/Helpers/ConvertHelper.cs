namespace Shared.Lib.Helpers;

public static class ConvertHelper
{
    public static T ConvertValue<T>(string value)
    {

        try
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(DateTime))
            {
                return (T)(object)DateTime.Parse(value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)int.Parse(value);
            }
            else if (typeof(T) == typeof(double))
            {
                return (T)(object)double.Parse(value);
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)bool.Parse(value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (T)(object)decimal.Parse(value);
            }
            else if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), value);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to convert value '{value}' to type {typeof(T)}", ex);
        }
    }
}