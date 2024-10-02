using Consul;
using GameLib.Domain.Abstractions;
using Shared.Domain.Entities;
using System.ComponentModel;
using System.Reflection;

namespace GameLib.Domain.Generators;

public class EntityGenerator
{
    public EntityMetadata GenerateEntityMetadata(Type type)
    {
        var entityMetadata = new EntityMetadata
        {
            Name = type.Name,
            Description = GetClassDescription(type)
        };

        foreach (var property in type.GetProperties())
        {
            var propertyMetadata = new PropertyMetadata
            {
                Name = property.Name,
                Type = IsDbEnum(property.PropertyType) ? "enum" : GetPropertyDataType(property),
                Description = GetPropertyDescription(property),
                Id = $"{type.Name}.{property.Name}" // Generate a unique ID for each property
            };

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

        if (IsDbEnum(type))
            return "enum";  // Treat DbEnum as an enum in the frontend
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
        var metaDescription = (MetaDescription)Attribute.GetCustomAttribute(type, typeof(MetaDescription));
        if (metaDescription != null)
            return metaDescription.Description;

        return (Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? "";
    }

    private string GetPropertyDescription(PropertyInfo property)
    {
        var metaDescription = property.GetCustomAttribute<MetaDescription>();
        if (metaDescription != null)
            return metaDescription.Description;

        return property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
    }
    private bool IsDbEnum(Type type)
    {
        return type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(DbEnum<,>);
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
    public string Id { get; set; }  // Unique identifier for each property
    public EntityMetadata? GenericTypeMetadata { get; set; }
}

public class ConfigurationMetadata
{
    public Dictionary<string, EntityMetadata> NavigationPropertyCache { get; set; }
}

public class MetaDescription(string description) : Attribute
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

class GameConfiguration : GameLib.Domain.Entities.Configuration
{
    public List<Round> Rounds { get; set; }
}

//class T
//{
//    public T()
//    {

//        var conf = new GameConfiguration
//        {
//            Rounds =
//            new List<Round>(){
//                        new Round()
//                        {
//                            Id = 1,
//                            NextPrizeIndex = 0,
//                            Sequence    = [],
//                        },
//                        new Round()
//                        {
//                            Id = 1,
//                            NextPrizeIndex = 0,
//                            Sequence    = [],
//                            Prizes = [new(){PrizeGroup}]
//                        },

//        }
//        };


//    }
//}