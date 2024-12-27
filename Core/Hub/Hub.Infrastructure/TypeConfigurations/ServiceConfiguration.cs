using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Hub.Domain.Entities;

namespace Hub.Infrastructure.TypeConfigurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        // Many-to-Many Relationship between Segments and Promotions
        builder.HasMany(s => s.Promotions)
               .WithMany(p => p.Services)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Promotion)}{nameof(Service)}Mappings",
                    j => j.HasOne<Promotion>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Promotion)}{nameof(Promotion.Id)}")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Service>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Service)}{nameof(Service.Id)}")
                          .OnDelete(DeleteBehavior.Cascade)
                );
    }
}