using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Domain.Entities;

namespace Shared.Infrastructure.TypeConfigurations;

public class PrizeGroupTypeConfiguration : IEntityTypeConfiguration<Base.PrizeGroup>
{
    public void Configure(EntityTypeBuilder<Base.PrizeGroup> builder)
    {
        builder.Property(e => e.Sequence)
                .HasConversion(new ValueConverter<List<int>, string>(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
                );
    }
}