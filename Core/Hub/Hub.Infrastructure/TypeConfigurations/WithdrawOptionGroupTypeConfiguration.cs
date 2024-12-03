using Hub.Domain.Entities;
using Hub.Domain.Entities.Coins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class WithdrawOptionGroupTypeConfiguration : IEntityTypeConfiguration<WithdrawOptionGroup>
{
    public void Configure(EntityTypeBuilder<WithdrawOptionGroup> builder)
    {
        // Many-to-Many Relationship between WithdrawOptionGroups and Coins
        builder.HasMany(w => w.OutCoins)
               .WithMany(oc => oc.WithdrawOptionGroups)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Coin)}{nameof(WithdrawOptionGroup)}Mappings",
                    join => join.HasOne<OutCoin>()
                                .WithMany()
                                .HasForeignKey($"{nameof(Coin)}{nameof(Coin.Id)}")
                                .OnDelete(DeleteBehavior.Cascade),
                    join => join.HasOne<WithdrawOptionGroup>()
                                .WithMany()
                                .HasForeignKey($"{nameof(WithdrawOptionGroup)}{nameof(WithdrawOptionGroup.Id)}")
                                .OnDelete(DeleteBehavior.Cascade)
                );
    }
}