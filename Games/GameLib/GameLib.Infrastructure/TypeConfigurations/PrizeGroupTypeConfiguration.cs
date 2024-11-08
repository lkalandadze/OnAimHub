using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GameLib.Domain.Abstractions;

namespace GameLib.Infrastructure.TypeConfigurations;

public class PrizeGroupTypeConfiguration<TPrizeGroup, TPrize> : IEntityTypeConfiguration<TPrizeGroup>
    where TPrizeGroup : BasePrizeGroup<TPrize>
    where TPrize : BasePrize
{
    public void Configure(EntityTypeBuilder<TPrizeGroup> builder)
    {
        builder.Property(e => e.Sequence)
                .HasConversion(new ValueConverter<List<int>, string>(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
                );
    }
}