using GameLib.Domain.Abstractions;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameLib.Infrastructure.TypeConfigurations;

public class GameConfigurationTypeConfiguration<TBaseConfiguration> : IEntityTypeConfiguration<TBaseConfiguration>
    where TBaseConfiguration : GameConfiguration<TBaseConfiguration>
{
    public void Configure(EntityTypeBuilder<TBaseConfiguration> builder)
    {
        // Iterate through all foreign keys related to GameConfiguration
        foreach (var foreignKey in builder.Metadata.GetForeignKeys())
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
        }
    }
}