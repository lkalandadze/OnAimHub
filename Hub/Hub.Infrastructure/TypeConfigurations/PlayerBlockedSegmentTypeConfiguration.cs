using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class PlayerBlockedSegmentTypeConfiguration : IEntityTypeConfiguration<PlayerBlockedSegment>
{
    public void Configure(EntityTypeBuilder<PlayerBlockedSegment> builder)
    {
        builder.Ignore(e => e.Id);

        builder.HasKey(ps => new { ps.PlayerId, ps.SegmentId });

        builder.HasOne(ps => ps.Player)
               .WithMany(p => p.PlayerBlockedSegments)
               .HasForeignKey(ps => ps.PlayerId);

        builder.HasOne(ps => ps.Segment)
               .WithMany(s => s.PlayerBlockedSegments)
               .HasForeignKey(ps => ps.SegmentId);
    }
}