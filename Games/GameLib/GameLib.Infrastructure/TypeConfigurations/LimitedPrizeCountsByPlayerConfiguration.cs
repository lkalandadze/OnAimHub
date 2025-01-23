using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Infrastructure.TypeConfigurations;

public class LimitedPrizeCountsByPlayerConfiguration : IEntityTypeConfiguration<LimitedPrizeCountsByPlayer>
{
    public void Configure(EntityTypeBuilder<LimitedPrizeCountsByPlayer> builder)
    {
        builder.Ignore(pp => pp.Id);

        builder.HasKey(pp => new { pp.PlayerId, pp.PrizeId });

        builder.Property(pp => pp.PlayerId)
               .IsRequired();

        builder.Property(pp => pp.PrizeId)
               .IsRequired();

        builder.Property(pp => pp.Count)
               .IsRequired()
               .HasDefaultValue(0);
    }
}