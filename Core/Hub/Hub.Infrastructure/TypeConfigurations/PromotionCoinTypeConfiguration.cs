using Hub.Domain.Entities.PromotionCoins;
using Hub.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class PromotionCoinTypeConfiguration : IEntityTypeConfiguration<PromotionCoin>
{
    public void Configure(EntityTypeBuilder<PromotionCoin> builder)
    {
        // Configure TPH inheritance
        builder.HasDiscriminator<CoinType>(nameof(CoinType))
               .HasValue<PromotionCoin>(CoinType.Default) // Default value for the base type
               .HasValue<PromotionIncomingCoin>(CoinType.Incomming)
               .HasValue<PromotionOutgoingCoin>(CoinType.Outgoing)
               .HasValue<PromotionInternalCoin>(CoinType.Internal)
               .HasValue<PromotionPrizeCoin>(CoinType.Prize);
    }
}