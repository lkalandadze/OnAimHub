using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GameLib.Domain.Entities;
using Shared.Lib.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameLib.Infrastructure.DataAccess;

public abstract class SharedGameConfigDbContext(DbContextOptions options) : DbContext(options)
{

}

public abstract class SharedGameConfigDbContext<T> : SharedGameConfigDbContext where T : GameConfiguration<T>
{
    public SharedGameConfigDbContext(DbContextOptions<SharedGameConfigDbContext> options)
        : base(options)
    {

    }

    public DbSet<T> GameConfigurations { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<GameSetting> GameSettings { get; set; }
    //public DbSet<PrizeType> PrizeTypes { get; set; }
    //public DbSet<Coin> Coins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var clrType = entityType.ClrType;

            foreach (var property in clrType.GetProperties())
            {
                var attribute = property.GetCustomAttributes()
                    .FirstOrDefault(attr => attr is ListToStringConverterAttribute<bool>);

                if (attribute is ListToStringConverterAttribute<bool> collectionConverterAttribute)
                {
                    var entityBuilder = modelBuilder.Entity(clrType);
                    var propertyMethod = entityBuilder.GetType()
                                .GetMethods().Single(x => x.Name == nameof(EntityTypeBuilder.Property) && x.IsGenericMethod);

                    var genericPropertyMethod = propertyMethod.MakeGenericMethod(attribute.GetType().GenericTypeArguments.First());

                    var propertyBuilder = entityBuilder.Property<List<bool>>(property.Name);

                    var configureMethod = attribute.GetType()
                        .GetMethod(nameof(ListToStringConverterAttribute<bool>.ConfigureConverter));

                    configureMethod?.Invoke(attribute, new object[] { propertyBuilder });
                }
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SharedGameConfigDbContext<>))!);

        base.OnModelCreating(modelBuilder);
    }
}