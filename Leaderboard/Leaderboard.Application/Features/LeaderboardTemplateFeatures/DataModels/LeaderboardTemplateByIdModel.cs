using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.DataModels;

public sealed record LeaderboardTemplateByIdModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public List<LeaderboardPrizeModel> Prizes { get; set; } = new();

    public static LeaderboardTemplateByIdModel MapFrom(LeaderboardTemplate leaderboardTemplate)
    {
        return new LeaderboardTemplateByIdModel
        {
            Id = leaderboardTemplate.Id,
            Name = leaderboardTemplate.Name,
            Description = leaderboardTemplate.Description,
            StartTime = leaderboardTemplate.StartTime,
            AnnounceIn = leaderboardTemplate.AnnounceIn,
            StartIn = leaderboardTemplate.StartIn,
            EndIn = leaderboardTemplate.EndIn,
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