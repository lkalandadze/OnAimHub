using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.DataModels;

public sealed record LeaderboardTemplatesModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public static LeaderboardTemplatesModel MapFrom(LeaderboardTemplate leaderboardTemplate)
    {
        return new LeaderboardTemplatesModel
        {
            Id = leaderboardTemplate.Id,
            Name = leaderboardTemplate.Name,
            StartTime = leaderboardTemplate.StartTime,
            AnnounceIn = leaderboardTemplate.AnnounceIn,
            StartIn = leaderboardTemplate.StartIn,
            EndIn = leaderboardTemplate.EndIn
        };
    }
}