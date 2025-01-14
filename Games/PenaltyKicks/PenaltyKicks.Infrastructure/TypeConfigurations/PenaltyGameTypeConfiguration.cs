using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PenaltyKicks.Domain.Entities;

namespace PenaltyKicks.Infrastructure.TypeConfigurations;

public class PenaltyGameTypeConfiguration : IEntityTypeConfiguration<PenaltyGame>
{
    public void Configure(EntityTypeBuilder<PenaltyGame> builder)
    {
        builder.Property(e => e.KickSequence)
                .HasConversion(new ValueConverter<List<bool>, string>(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(bool.Parse).ToList())
                );
    }
}