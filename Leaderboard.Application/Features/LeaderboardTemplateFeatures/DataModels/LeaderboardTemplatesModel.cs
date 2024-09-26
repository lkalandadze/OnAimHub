using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.DataModels;

public sealed record LeaderboardTemplatesModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public static LeaderboardTemplatesModel MapFrom(LeaderboardTemplate leaderboardTemplate)
    {
        return new LeaderboardTemplatesModel
        {
            Id = leaderboardTemplate.Id,
            Name = leaderboardTemplate.Name,
            JobType = leaderboardTemplate.JobType,
            StartTime = leaderboardTemplate.StartTime,
            DurationInDays = leaderboardTemplate.DurationInDays,
            AnnouncementLeadTimeInDays = leaderboardTemplate.AnnouncementLeadTimeInDays,
        };
    }
}