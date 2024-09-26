using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.DataModels;

public sealed record LeaderboardTemplateByIdModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public int CreationLeadTimeInDays { get; set; }
    public List<LeaderboardPrizeModel> Prizes { get; set; } = new();

    public static LeaderboardTemplateByIdModel MapFrom(LeaderboardTemplate leaderboardTemplate)
    {
        return new LeaderboardTemplateByIdModel
        {
            Id = leaderboardTemplate.Id,
            Name = leaderboardTemplate.Name,
            JobType = leaderboardTemplate.JobType,
            StartTime = leaderboardTemplate.StartTime,
            DurationInDays = leaderboardTemplate.DurationInDays,
            AnnouncementLeadTimeInDays = leaderboardTemplate.AnnouncementLeadTimeInDays,
            CreationLeadTimeInDays = leaderboardTemplate.CreationLeadTimeInDays,
            Prizes = leaderboardTemplate.LeaderboardTemplatePrizes?.Select(prize => new LeaderboardPrizeModel
            {
                Id = prize.Id,
                StartRank = prize.StartRank,
                EndRank = prize.EndRank,
                PrizeId = prize.PrizeId,
                Amount = prize.Amount
            }).ToList() ?? new List<LeaderboardPrizeModel>()
        };
    }
}