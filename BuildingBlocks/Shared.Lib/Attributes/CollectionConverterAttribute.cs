using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;

namespace Shared.Lib.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public abstract class ListConverterAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Property)]
public abstract class CollectionConverterAttribute<T> : ListConverterAttribute
{
    public abstract void ConfigureConverter(PropertyBuilder<T> builder);
}

public class ListToStringConverterAttribute<T> : CollectionConverterAttribute<List<T>>
{
    public override void ConfigureConverter(PropertyBuilder<List<T>> builder)
    {
        builder.HasConversion(new ValueConverter<List<T>, string>(
                   v => string.Join(',', v),
                   v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Parse).ToList())
               );
    }

    private static T Parse(string value)
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter != null && converter.CanConvertFrom(typeof(string)))
        {
            return (T)converter.ConvertFromInvariantString(value)!;
        }

        throw new InvalidOperationException($"Cannot convert string to type {typeof(T).Name}");
    }
}