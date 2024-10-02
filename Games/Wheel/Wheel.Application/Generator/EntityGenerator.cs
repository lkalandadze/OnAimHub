using GameLib.Domain.Abstractions;
using System.ComponentModel;
using System.Reflection;

namespace Wheel.Application.Generator;

public class MetadataGenerator
{
    public EntityMetadata GenerateEntityMetadata(Type type)
    {
        var entityMetadata = new EntityMetadata
        {
            Name = type.Name,
            Description = GetClassDescription(type) // This could be filled with custom logic or annotations
        };

        foreach (var property in type.GetProperties())
        {
            var propertyMetadata = new PropertyMetadata
            {
                Name = property.Name,
                Type = IsCollection(property)
                        ? "List" // Mark as List if it's a collection
                        : property.PropertyType.Name,
                Description = GetPropertyDescription(property) // Optionally fetch description
            };

            // Check if the property is a generic type and a collection
            if (IsCollection(property))
            {
                var genericType = property.PropertyType.GetGenericArguments().FirstOrDefault();
                if (genericType != null)
                {
                    propertyMetadata.GenericTypeMetadata = GenerateEntityMetadata(genericType);
                }
            }

            entityMetadata.Properties.Add(propertyMetadata);
        }

        return entityMetadata;
    }

    private static bool IsCollection(PropertyInfo property)
    {
        return property.PropertyType.IsGenericType
            && (typeof(List<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition())
            || typeof(ICollection<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()));
    }

    private string GetPropertyDataType(PropertyInfo property)
    {
        var type = property.PropertyType;

        // Determine frontend data type
        if (type == typeof(string))
            return "text";
        if (type == typeof(int) || type == typeof(int?) ||
            type == typeof(long) || type == typeof(long?))
            return "number";
        if (type == typeof(bool) || type == typeof(bool?))
            return "boolean";
        if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
            return "date";

        // Fallback for other types
        return "text"; // Default to text for unknown types
    }

    private string GetClassDescription(Type type)
    {
        return (Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? "";
    }

    private string GetPropertyDescription(PropertyInfo property)
    {
        return property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
    }
}
public class EntityMetadata
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PropertyMetadata> Properties { get; set; } = new List<PropertyMetadata>();
}

public class PropertyMetadata
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; } // Could be filled with annotations or custom logic
    public EntityMetadata? GenericTypeMetadata { get; set; }
}

public class ConfigurationMetadata
{
    public Dictionary<string, EntityMetadata> NavigationPropertyCache { get; set; }
}

public class F(string description) : Attribute
{
    public string Description { get; set; } = description;
}

[Description("This is a round description!")]
public class Round : BasePrizeGroup<TestPrize>
{
    [Description("Round index")]
    public int Index { get; set; }
    public ICollection<TestPrize> Prizes { get; set; }
}

public class TestPrize : BasePrize<Round>
{
    [Description("This is a description of RoundId!")]
    public int RoundId { get; set; }
    public Round Round { get; set; }
}