using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infrastructure.TypeConfigurations;

public class WithdrawOptionGroupTypeConfiguration : IEntityTypeConfiguration<WithdrawOptionGroup>
{
    public void Configure(EntityTypeBuilder<WithdrawOptionGroup> builder)
    {

        // Configuration for PromotionCoins
        builder.HasMany(w => w.PromotionCoins)
               .WithMany(p => p.WithdrawOptionGroups)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(WithdrawOptionGroup)}{nameof(PromotionCoin)}Mappings",
                    j => j.HasOne<PromotionCoin>()
                        .WithMany()
                        .HasForeignKey($"{nameof(PromotionCoin)}{nameof(PromotionCoin.Id)}")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<WithdrawOptionGroup>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOptionGroup)}{nameof(WithdrawOptionGroup.Id)}")
                        .OnDelete(DeleteBehavior.Cascade)
                );

        // Configuration for CoinTemplates
        builder.HasMany(w => w.CoinTemplates)
               .WithMany(c => c.WithdrawOptionGroups)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(WithdrawOptionGroup)}{nameof(CoinTemplate)}Mappings",
                    j => j.HasOne<CoinTemplate>()
                        .WithMany()
                        .HasForeignKey($"{nameof(CoinTemplate)}{nameof(CoinTemplate.Id)}")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<WithdrawOptionGroup>()
                        .WithMany()
                        .HasForeignKey($"{nameof(WithdrawOptionGroup)}{nameof(WithdrawOptionGroup.Id)}")
                        .OnDelete(DeleteBehavior.Cascade)
                );
    }
}