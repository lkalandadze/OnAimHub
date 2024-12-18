using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using Shared.Lib.Helpers;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Infrastructure;

public abstract class BaseDbInitializer
{
    protected virtual async Task SeedDbEnums(DbContext dbContext, Assembly assembly)
    {
        var dbEnumTypes = assembly.GetTypes()
            .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(DbEnum<,>));

        foreach (var enumType in dbEnumTypes)
        {
            try
            {
                var dbSetMethod = dbContext.GetType()
                    .GetMethods()
                    .First(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0);

                var dbSet = dbSetMethod.MakeGenericMethod(enumType).Invoke(dbContext, null);

                if (dbSet == null)
                {
                    continue;
                }

                var ownTypeStaticProperties = enumType.GetProperties(BindingFlags.Public | BindingFlags.Static);

                foreach (var property in ownTypeStaticProperties)
                {
                    var enumValue = property.GetValue(null);

                    if (enumValue != null)
                    {
                        var enumInstanceName = property.Name;

                        var anyExistsMethod = typeof(Queryable).GetMethods()
                            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                            .MakeGenericMethod(enumType);

                        var parameter = Expression.Parameter(enumType);
                        var nameProperty = Expression.Property(parameter, "Name");
                        var condition = Expression.Equal(nameProperty, Expression.Constant(enumInstanceName));
                        var lambda = Expression.Lambda(condition, parameter);

                        var exists = (bool)anyExistsMethod.Invoke(null, [dbSet, lambda])!;

                        if (exists)
                        {
                            continue;
                        }

                        if (Activator.CreateInstance(enumType) is { } dbEnumInstance)
                        {
                            var idProperty = dbEnumInstance.GetType().GetProperties().First(x => x.Name == nameof(DbEnum<object>.Id));

                            if (idProperty != null && idProperty.PropertyType == typeof(string))
                            {
                                ReflectionHelper.SetProperty(dbEnumInstance, idProperty, enumInstanceName);
                            }

                            ReflectionHelper.SetProperty(dbEnumInstance, nameof(DbEnum<object>.Name), enumInstanceName);

                            var addMethod = dbSet.GetType().GetMethod("Add");
                            addMethod!.Invoke(dbSet, new[] { dbEnumInstance });
                        }
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding {enumType.Name}: {ex.Message}");
            }
        }
    }
}