using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;

public sealed record PlayerResultModel
{
    public int Id { get; set; }
    public int LeaderboardRecordId { get; set; }
    public int Amount { get; set; }
    public int Placement { get; set; }

    public static PlayerResultModel MapFrom(LeaderboardResult leaderboardProgress)
    {
        return new PlayerResultModel
        {
            Id = leaderboardProgress.Id,
            LeaderboardRecordId = leaderboardProgress.LeaderboardRecordId,
            Amount = leaderboardProgress.Amount,
            Placement = leaderboardProgress.Placement
        };
    }
}