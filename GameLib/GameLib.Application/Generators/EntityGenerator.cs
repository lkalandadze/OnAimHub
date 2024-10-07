using GameLib.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using OnAim.Lib.EntityExtension.GlobalAttributes;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using Shared.Domain.Entities;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace GameLib.Application.Generators;

public class EntityGenerator
{
    public EntityMetadata GenerateEntityMetadata(Type type)
    {
        if (AttributeHelper.IsHidden(type))
        {
            return null;
        }

        var entityMetadata = new EntityMetadata
        {
            Name = type.Name,
            Description = GetClassDescription(type)
        };

        foreach (var property in type.GetProperties())
        {
            if (AttributeHelper.IsHidden(property))
            {
                continue;
            }

            var propertyMetadata = new PropertyMetadata
            {
                Name = property.Name,
                Type = IsCollection(property.PropertyType)
                        ? "List"
                        : property.PropertyType.Name,
                Description = GetPropertyDescription(property)
            };

            if (IsCollection(property.PropertyType))
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

    private static bool IsCollection(Type type)
    {
        return type.IsGenericType
            && (typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition())
            || typeof(ICollection<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
    }

    private string GetPropertyDataType(PropertyInfo property)
    {
        var type = property.PropertyType;

        if (IsDbEnum(type))
            return "enum";
        if (type == typeof(string))
            return "text";
        if (type == typeof(int) || type == typeof(int?) ||
            type == typeof(long) || type == typeof(long?))
            return "number";
        if (type == typeof(bool) || type == typeof(bool?))
            return "boolean";
        if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
            return "date";
        if (IsCollection(type))
            return "List";
        return "text";
    }

    private string GetClassDescription(Type type)
    {
        var metaDescription = (GlobalDescriptionAttribute)Attribute.GetCustomAttribute(type, typeof(GlobalDescriptionAttribute));
        if (metaDescription != null)
            return metaDescription.Description;

        return (Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? "";
    }

    private string GetPropertyDescription(PropertyInfo property)
    {
        var metaDescription = property.GetCustomAttribute<GlobalDescriptionAttribute>();
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

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> IncludeNotHiddenAll<TEntity>(this IQueryable<TEntity> query, params Type[] excludedTypes) where TEntity : class
    {
        return IncludeRecursive(query, typeof(TEntity), "", excludedTypes);
    }

    private static IQueryable<TEntity> IncludeRecursive<TEntity>(IQueryable<TEntity> query, Type entityType, string path, params Type[] excludedTypes) where TEntity : class
    {
        // Get properties that are navigable (ignore scalar properties)
        var navigations = entityType.GetProperties()
            .Where(p => IsNavigableProperty(p) && p.Name != "Id") // Only navigable properties
            .Where( p => p.GetCustomAttribute<IgnoreIncludeAllAttribute>() == null)
            .ToList();

        foreach (var navigation in navigations)
        {
            // Build the include path for the property
            var navigationPath = string.IsNullOrEmpty(path) ? navigation.Name : $"{path}.{navigation.Name}";

            if (navigation.PropertyType.IsGenericType && navigation.PropertyType.GetGenericArguments().First().GetCustomAttribute<IgnoreIncludeAllAttribute>() != null)
            {
                continue;
            }

            // Only include valid navigation properties (classes or collections)
            query = query.Include(navigationPath);

            Type navigationType = navigation.PropertyType;

            // Check if the property is a collection (e.g., List<T>)
            if (IsCollection(navigation.PropertyType))
            {
                navigationType = navigation.PropertyType.IsGenericType
                    ? navigation.PropertyType.GetGenericArguments().FirstOrDefault()
                    : navigation.PropertyType.GetElementType();

                IncludeRecursive(query, navigationType, navigationPath);
            }

            //// Recursively include properties for navigation types
            //if (navigationType != null && navigationType != typeof(string))
            //{
            //    query = IncludeRecursive(query, navigationType, navigationPath);
            //}
        }

        return query;
    }

    private static bool IsNavigableProperty(PropertyInfo property)
    {
        var type = property.PropertyType;

        if (type.IsPrimitive || type == typeof(string) || type.IsEnum || type == typeof(DateTime) || type == typeof(decimal))
        {
            return false;
        }

        return type.IsClass || IsCollection(type);
    }
    private static bool IsCollection(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }
}
