using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameLib.Infrastructure.TypeConfigurations;

public class SegmentTypeConfiguration : IEntityTypeConfiguration<Segment>
{
    public void Configure(EntityTypeBuilder<Segment> builder)
    {
        builder.HasOne(s => s.Configuration)
               .WithMany(s => s.Segments)
               .HasForeignKey(s => s.ConfigurationId)
               .IsRequired(false);
    }
}