﻿#nullable disable

using CheckmateValidations;
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
    public EntityMetadata? GenerateEntityMetadata(Type type)
    {
        if (AttributeHelper.IsHidden(type))
        {
            return null;
        }

        var entityMetadata = new EntityMetadata
        {
            Name = type.Name,
            Description = GetClassDescription(type),
            Validations = CheckmateValidations.Checkmate.GetRootCheckContainers(type),
            Properties = new List<PropertyMetadata>()
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
                Type = IsCollection(property.PropertyType) ? "List" : property.PropertyType.Name,
                Description = GetPropertyDescription(property),
            };

            if (IsCollection(property.PropertyType))
            {
                var genericType = property.PropertyType.GetGenericArguments().FirstOrDefault();
                if (genericType != null)
                {
                    propertyMetadata.GenericTypeMetadata = GenerateEntityMetadata(genericType);
                }
            }
            else
            {
                propertyMetadata.GenericTypeMetadata = null;
            }

            entityMetadata.Properties.Add(propertyMetadata);
        }

        return entityMetadata;
    }

    private static bool IsCollection(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }

    private string GetPropertyDataType(PropertyInfo property)
    {
        var type = property.PropertyType;

        if (IsDbEnum(type))
        {
            return "enum";
        }
        else if (type == typeof(string))
        {
            return "text";
        }
        else if (type == typeof(int) || type == typeof(int?) || type == typeof(long) || type == typeof(long?))
        {
            return "number";
        }
        else if (type == typeof(bool) || type == typeof(bool?))
        {
            return "boolean";
        }
        else if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
        {
            return "date";
        }
        else if (IsCollection(type))
        {
            return "List";
        }
            
        return "text";
    }

    private string GetClassDescription(Type type)
    {
        var metaDescription = Attribute.GetCustomAttribute(type, typeof(GlobalDescriptionAttribute)) as GlobalDescriptionAttribute;
        
        if (metaDescription != null)
        {
            return metaDescription.Description;

        }
        return (Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? "";
    }

    private string GetPropertyDescription(PropertyInfo property)
    {
        var metaDescription = property.GetCustomAttribute<GlobalDescriptionAttribute>();

        if (metaDescription != null)
        {
            return metaDescription.Description;
        }

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
    public List<CheckContainer> Validations { get; set; }
    public List<PropertyMetadata> Properties { get; set; } = new List<PropertyMetadata>();
}

public class PropertyMetadata
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public EntityMetadata? GenericTypeMetadata {  get;  set; }
}

public class ConfigurationMetadata
{
    public Dictionary<string, EntityMetadata> NavigationPropertyCache { get; set; }
}

//[Description("This is a round description!")]
//public class Round : BasePrizeGroup<TestPrize>
//{
//    [Description("Round index")]
//    public int Index { get; set; }

//    public ICollection<TestPrize> Prizes { get; set; }
//}

//public class TestPrize : BasePrize<Round>
//{
//    [Description("This is a description of RoundId!")]
//    public int RoundId { get; set; }

//    public Round Round { get; set; }
//}

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> IncludeNotHiddenAll<TEntity>(this IQueryable<TEntity> query, params Type[] excludedTypes) where TEntity : class
    {
        return IncludeRecursive(query, typeof(TEntity), "", excludedTypes);
    }

    private static IQueryable<TEntity> IncludeRecursive<TEntity>(IQueryable<TEntity> query, Type entityType, string path, params Type[] excludedTypes) where TEntity : class
    {
        var navigations = entityType.GetProperties()
            .Where(p => IsNavigableProperty(p) && p.Name != "Id")
            .Where( p => p.GetCustomAttribute<IgnoreIncludeAllAttribute>() == null)
            .ToList();

        foreach (var navigation in navigations)
        {
            var navigationPath = string.IsNullOrEmpty(path) ? navigation.Name : $"{path}.{navigation.Name}";

            if (navigation.PropertyType.IsGenericType && navigation.PropertyType.GetGenericArguments().First().GetCustomAttribute<IgnoreIncludeAllAttribute>() != null)
            {
                continue;
            }

            query = query.Include(navigationPath);

            Type navigationType = navigation.PropertyType;

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
