using OnAim.Lib.EntityExtension.GlobalAttributes;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using Shared.Domain.Entities;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace GameLib.Application.Generators;

public class CommandGenerator
{
    public CreateEntityMetadata GenerateCreateEntityMetadata(Type type)
    {
        if (AttributeHelper.IsHidden(type))
        {
            return null;
        }

        var entityMetadata = new CreateEntityMetadata
        {
            Name = type.Name,
            Description = GetClassDescription(type),
            Properties = new List<CreatePropertyMetadata>()
        };

        foreach (var property in type.GetProperties())
        {
            if (AttributeHelper.IsHidden(property))
            {
                continue;
            }

            var propertyMetadata = new CreatePropertyMetadata
            {
                Name = property.Name,
                Type = IsCollection(property.PropertyType)
                        ? "List"
                        : property.PropertyType.Name
            };

            if (IsCollection(property.PropertyType))
            {
                var genericType = property.PropertyType.GetGenericArguments().FirstOrDefault();
                if (genericType != null)
                {
                    propertyMetadata.GenericTypeMetadata = GenerateCreateEntityMetadata(genericType);
                }
            }

            entityMetadata.Properties.Add(propertyMetadata);
        }

        return entityMetadata;
    }

    public string GenerateCreateCommandWithDtos(Type entityType)
    {
        var entityMetadata = GenerateCreateEntityMetadata(entityType);

        var commandClassName = $"Create{entityMetadata.Name}Command";
        var commandClassBuilder = new StringBuilder();

        // Start generating the command class
        commandClassBuilder.AppendLine("using MediatR;");
        commandClassBuilder.AppendLine("using System.Collections.Generic;");
        commandClassBuilder.AppendLine($"public class {commandClassName} : IRequest<int>");
        commandClassBuilder.AppendLine("{");

        // Add properties from GameConfiguration (Name, Value, IsActive)
        commandClassBuilder.AppendLine($"    public string Name {{ get; set; }}  // GlobalDescription(\"Name of the configuration\")");
        commandClassBuilder.AppendLine($"    public int Value {{ get; set; }}    // GlobalDescription(\"Value of the configuration\")");
        commandClassBuilder.AppendLine($"    public bool IsActive {{ get; set; }} // GlobalDescription(\"IsActive of the configuration\")");

        foreach (var property in entityMetadata.Properties)
        {
            if (property.Name == "Id")
            {
                // Only include Id if it is of type string
                if (property.Type == "String")
                {
                    commandClassBuilder.AppendLine($"    public {property.Type} {property.Name} {{ get; set; }}");
                }
            }
            else if (property.Type == "List" && property.GenericTypeMetadata != null)
            {
                // For collections, generate DTOs for nested types
                var collectionDtoClassName = $"{property.GenericTypeMetadata.Name}Dto";
                commandClassBuilder.AppendLine($"    public ICollection<{collectionDtoClassName}> {property.Name} {{ get; set; }}");

                // Generate DTO class for collection type
                commandClassBuilder.AppendLine(GenerateDto(property.GenericTypeMetadata));
            }
            else
            {
                // For basic properties, generate their fields in the command
                commandClassBuilder.AppendLine($"    public {property.Type} {property.Name} {{ get; set; }}");
            }
        }

        commandClassBuilder.AppendLine("}");

        // Return both the command and DTO classes as a combined result
        return commandClassBuilder.ToString();
    }

    // Method to generate DTOs for nested collections
    private string GenerateDto(CreateEntityMetadata entityMetadata)
    {
        var dtoClassName = $"{entityMetadata.Name}Dto";
        var dtoBuilder = new StringBuilder();

        dtoBuilder.AppendLine($"public class {dtoClassName}");
        dtoBuilder.AppendLine("{");

        foreach (var property in entityMetadata.Properties)
        {
            if (property.Name == "Id")
            {
                // Only include Id if it is of type string
                if (property.Type == "String")
                {
                    dtoBuilder.AppendLine($"    public {property.Type} {property.Name} {{ get; set; }}");
                }
            }
            else if (property.Type == "List" && property.GenericTypeMetadata != null)
            {
                var nestedDtoClassName = $"{property.GenericTypeMetadata.Name}Dto";
                dtoBuilder.AppendLine($"    public ICollection<{nestedDtoClassName}> {property.Name} {{ get; set; }}");
            }
            else
            {
                dtoBuilder.AppendLine($"    public {property.Type} {property.Name} {{ get; set; }}");
            }
        }

        dtoBuilder.AppendLine("}");

        return dtoBuilder.ToString(); // Return the generated DTO class as a string
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

    public class CreateEntityMetadata
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CreatePropertyMetadata> Properties { get; set; }
    }

    public class CreatePropertyMetadata
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public CreateEntityMetadata GenericTypeMetadata { get; set; }
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
