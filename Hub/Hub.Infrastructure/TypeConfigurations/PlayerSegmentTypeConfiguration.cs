using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Hub.Infrastructure.TypeConfigurations;

public class PlayerSegmentTypeConfiguration : IEntityTypeConfiguration<PlayerSegment>
{
    public void Configure(EntityTypeBuilder<PlayerSegment> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(s => s.AddedByUserId)
               .IsRequired(false);

        builder.HasOne(ps => ps.Player)
               .WithMany(p => p.PlayerSegments)
               .HasForeignKey(ps => ps.PlayerId);

        builder.HasOne(ps => ps.Segment)
               .WithMany(s => s.PlayerSegments)
               .HasForeignKey(ps => ps.SegmentId);
    }
}