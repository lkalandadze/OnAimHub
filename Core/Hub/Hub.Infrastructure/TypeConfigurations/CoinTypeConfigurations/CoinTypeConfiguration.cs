using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations.CoinTypeConfigurations;

public class CoinTypeConfiguration : IEntityTypeConfiguration<Coin>
{
    public void Configure(EntityTypeBuilder<Coin> builder)
    {
        // Configure TPH inheritance
        builder.HasDiscriminator<CoinType>(nameof(CoinType))
               .HasValue<Coin>(CoinType.Default) // Default value for the base type
               .HasValue<InCoin>(CoinType.In)
               .HasValue<OutCoin>(CoinType.Out)
               .HasValue<InternalCoin>(CoinType.Internal)
               .HasValue<AssetCoin>(CoinType.Asset);
    }
}