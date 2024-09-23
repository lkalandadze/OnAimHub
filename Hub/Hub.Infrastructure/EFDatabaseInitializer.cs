using Hub.Domain.Entities;
using Hub.Domain.Enum;
using Hub.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace Hub.Infrastructure;

public class EFDatabaseInitializer
{
    //TODO: კონფიგურაციების აპსეთინგებიდან ამოტანა

    public static void Initialize(IServiceScope serviceScope)
    {
        var dbContext = serviceScope.ServiceProvider.GetService<HubDbContext>();

        if (dbContext == null)
        {
            return;
        }

        _ = new EFDatabaseInitializer();

        SeedDbEnums(dbContext).Wait();
        SeedJobs(dbContext).Wait();
        SeedDefaultSegment(dbContext).Wait();
        SeedGames(dbContext).Wait();
    }

    protected static async Task SeedDbEnums(HubDbContext dbContext)
    {
        var assembly = Assembly.Load("Hub.Domain");

        var dbEnumTypes = assembly.GetTypes()
            .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(DbEnum<>));

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

                var staticProperties = enumType.GetProperties(BindingFlags.Public | BindingFlags.Static);

                foreach (var property in staticProperties)
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

                        var exists = (bool)anyExistsMethod.Invoke(null, new object[] { dbSet, lambda })!;

                        if (exists)
                        {
                            continue;
                        }

                        var dbEnumInstance = Activator.CreateInstance(enumType) as DbEnum<int>;
                        dbEnumInstance!.Name = enumInstanceName;

                        var addMethod = dbSet.GetType().GetMethod("Add");
                        addMethod!.Invoke(dbSet, [dbEnumInstance]);
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

    //Temp?
    protected static async Task SeedGames(HubDbContext dbContext)
    {
        if (!dbContext.Games.Any())
        {
            var game = new Game("Wheel");

            await dbContext.Games.AddAsync(game);
            await dbContext.SaveChangesAsync();
        }
    }

    protected static async Task SeedDefaultSegment(HubDbContext dbContext)
    {
        if (!dbContext.Segments.Any())
        {
            var segment = new Segment("Default", string.Empty, 1);

            await dbContext.Segments.AddAsync(segment);
            await dbContext.SaveChangesAsync();
        }
    }

    protected static async Task SeedJobs(HubDbContext dbContext)
    {
        if (!dbContext.Jobs.Any(x => x.Name == "Reset Daily Progress"))
        {
            var job = new Job("Reset Daily Progress", "Clear player progress table daily", "OnAimCoin", true, null, null, JobType.Daily);

            await dbContext.Jobs.AddAsync(job);
            await dbContext.SaveChangesAsync();
        }
    }
}