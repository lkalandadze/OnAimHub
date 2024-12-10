using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Lib.SwaggerFilters;

public class PolymorphismSchemaFilter<TBaseType> : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(TBaseType))
        {
            var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof(TBaseType) && typeof(TBaseType).IsAssignableFrom(type))
                .ToList();

            schema.Discriminator = new OpenApiDiscriminator
            {
                PropertyName = "Discriminator",
                Mapping = derivedTypes.ToDictionary(
                    derivedType => derivedType.Name,
                    derivedType => $"#/components/schemas/{derivedType.Name}")
            };

            schema.OneOf = derivedTypes.Select(derivedType =>
                context.SchemaGenerator.GenerateSchema(derivedType, context.SchemaRepository))
                .ToList();
        }
    }
}