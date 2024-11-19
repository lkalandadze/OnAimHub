using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infrastructure.TypeConfigurations;

public class SegmentTypeConfiguration : IEntityTypeConfiguration<Segment>
{
    public void Configure(EntityTypeBuilder<Segment> builder)
    {
        builder.Property(s => s.CreatedByUserId)
               .IsRequired(false);

        // Many-to-Many Relationship between Segment and Players
        builder.HasMany(s => s.Players)
               .WithMany(p => p.Segments)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Player)}{nameof(Segment)}Mappings",
                    j => j.HasOne<Player>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Player)}{nameof(Player.Id)}")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Segment>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Segment)}{nameof(Segment.Id)}")
                          .OnDelete(DeleteBehavior.Cascade)
                );

        // Many-to-Many Relationship for BlockedPlayers
        builder.HasMany(s => s.BlockedPlayers)
               .WithMany(p => p.BlockedSegments)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Player)}Blocked{nameof(Segment)}Mappings",
                    j => j.HasOne<Player>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Player)}{nameof(Player.Id)}")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Segment>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Segment)}{nameof(Segment.Id)}")
                          .OnDelete(DeleteBehavior.Cascade)
                );

        // Many-to-Many Relationship between Segments and Promotions
        builder.HasMany(s => s.Promotions)
               .WithMany(p => p.Segments)
               .UsingEntity<Dictionary<string, object>>(
                    $"{nameof(Promotion)}{nameof(Segment)}Mappings",
                    j => j.HasOne<Promotion>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Promotion)}{nameof(Promotion.Id)}")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Segment>()
                          .WithMany()
                          .HasForeignKey($"{nameof(Segment)}{nameof(Segment.Id)}")
                          .OnDelete(DeleteBehavior.Cascade)
                );
    }
}