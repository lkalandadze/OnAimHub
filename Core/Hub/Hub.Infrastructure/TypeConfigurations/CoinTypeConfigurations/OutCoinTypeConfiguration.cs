using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations.CoinTypeConfigurations;

public class OutCoinTypeConfiguration : IEntityTypeConfiguration<OutCoin>
{
    public void Configure(EntityTypeBuilder<OutCoin> builder)
    {
        builder.Property(oc => oc.Value)
               .HasColumnName($"{nameof(OutCoin)}_{nameof(OutCoin.Value)}");

        builder.HasDiscriminator<CoinType>(nameof(CoinType))
               .HasValue<OutCoin>(CoinType.Out);
    }
}