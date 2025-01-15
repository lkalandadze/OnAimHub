using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Hub.Infrastructure.TypeConfigurations.CoinTypeConfigurations;

public class OutCoinTypeConfiguration : IEntityTypeConfiguration<OutCoin>
{
    public void Configure(EntityTypeBuilder<OutCoin> builder)
    {
        builder.Property(oc => oc.Value)
               .HasColumnName($"{nameof(OutCoin)}_{nameof(OutCoin.Value)}");
    }
}