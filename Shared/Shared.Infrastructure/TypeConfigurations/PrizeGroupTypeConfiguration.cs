using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Domain.Entities;
using System.Reflection.Emit;

namespace Shared.Infrastructure.TypeConfigurations;

public class PrizeGroupTypeConfiguration : IEntityTypeConfiguration<PrizeGroup>
{
    public void Configure(EntityTypeBuilder<PrizeGroup> builder)
    {
        builder.Property(e => e.Sequence)
                .HasConversion(new ValueConverter<List<int>, string>(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
                );
    }
}