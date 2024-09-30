using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class PlayerSegmentTypeConfiguration : IEntityTypeConfiguration<PlayerSegment>
{
    public void Configure(EntityTypeBuilder<PlayerSegment> builder)
    {
        builder.Ignore(e => e.Id);

        builder.HasKey(ps => new { ps.PlayerId, ps.SegmentId });

        builder.HasOne(ps => ps.Player)
               .WithMany(p => p.PlayerSegments)
               .HasForeignKey(ps => ps.PlayerId);

        builder.HasOne(ps => ps.Segment)
               .WithMany(s => s.PlayerSegments)
               .HasForeignKey(ps => ps.SegmentId);
    }
}