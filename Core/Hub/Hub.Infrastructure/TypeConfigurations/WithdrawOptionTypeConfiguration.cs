using Hub.Domain.Entities;
using Hub.Domain.Entities.Coins;
using Hub.Domain.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class WithdrawOptionTypeConfiguration : IEntityTypeConfiguration<WithdrawOption>
{
    public void Configure(EntityTypeBuilder<WithdrawOption> builder)
    {
        // Many-to-Many Relationship between WithdrawOption and WithdrawOptionGroups
        builder.HasMany(w => w.WithdrawOptionGroups)
               .WithMany(g => g.WithdrawOptions)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(WithdrawOptionGroup)}Mappings",
                    j => j.HasOne<WithdrawOptionGroup>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOptionGroup)}{nameof(WithdrawOptionGroup.Id)}")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<WithdrawOption>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
                        .OnDelete(DeleteBehavior.Cascade)
                );

        // Many-to-Many Relationship between WithdrawOption and PromotionCoins
        builder.HasMany(w => w.OutCoins)
          .WithMany(c => c.WithdrawOptions)
          .UsingEntity<Dictionary<string, object>>(
               $"{nameof(Coin)}{nameof(WithdrawOption)}Mappings",
               j => j.HasOne<OutCoin>()
                   .WithMany()
                   .HasForeignKey($"{nameof(Coin)}{nameof(Coin.Id)}")
                   .OnDelete(DeleteBehavior.Cascade),
               j => j.HasOne<WithdrawOption>()
                   .WithMany()
                   .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
                   .OnDelete(DeleteBehavior.Cascade)
          );
    }
}