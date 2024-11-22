using Hub.Domain.Entities;
using Hub.Domain.Entities.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class WithdrawOptionTypeConfiguration : IEntityTypeConfiguration<WithdrawOption>
{
    public void Configure(EntityTypeBuilder<WithdrawOption> builder)
    {
        // Configuration for WithdrawOptionGroups
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

        builder.HasMany(w => w.CoinTemplates)
           .WithMany(c => c.WithdrawOptions)
           .UsingEntity<Dictionary<string, object>>(
                $"{nameof(WithdrawOption)}{nameof(CoinTemplate)}Mappings",
                j => j.HasOne<CoinTemplate>()
                    .WithMany()
                    .HasForeignKey($"{nameof(CoinTemplate)}{nameof(CoinTemplate.Id)}")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<WithdrawOption>()
                    .WithMany()
                    .HasForeignKey($"{nameof(WithdrawOption)}{nameof(WithdrawOption.Id)}")
                    .OnDelete(DeleteBehavior.Cascade)
            );
    }
}