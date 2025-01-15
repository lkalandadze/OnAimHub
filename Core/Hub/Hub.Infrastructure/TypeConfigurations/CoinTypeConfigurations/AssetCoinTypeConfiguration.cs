using Hub.Domain.Entities.Coins;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infrastructure.TypeConfigurations.CoinTypeConfigurations;

public class AssetCoinTypeConfiguration : IEntityTypeConfiguration<AssetCoin>
{
    public void Configure(EntityTypeBuilder<AssetCoin> builder)
    {
        builder.Property(ac => ac.Value)
               .HasColumnName($"{nameof(AssetCoin)}_{nameof(AssetCoin.Value)}");
    }
}