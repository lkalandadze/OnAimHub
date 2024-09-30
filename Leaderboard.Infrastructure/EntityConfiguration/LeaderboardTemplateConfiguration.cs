using Leaderboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Leaderboard.Infrastructure.EntityConfiguration;

public class LeaderboardTemplateConfiguration : IEntityTypeConfiguration<LeaderboardTemplate>
{
    public void Configure(EntityTypeBuilder<LeaderboardTemplate> builder)
    {
    }
}
