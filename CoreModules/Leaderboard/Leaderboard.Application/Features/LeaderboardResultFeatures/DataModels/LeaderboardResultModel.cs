using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;

public sealed record LeaderboardResultModel
{
    public int Id { get; set; }
    public int LeaderboardRecordId { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Amount { get; set; }
    public int Placement { get; set; }

    public static LeaderboardResultModel MapFrom(LeaderboardResult leaderboardProgress)
    {
        return new LeaderboardResultModel
        {
            Id = leaderboardProgress.Id,
            LeaderboardRecordId = leaderboardProgress.LeaderboardRecordId,
            PlayerId = leaderboardProgress.PlayerId,
            PlayerUsername = leaderboardProgress.PlayerUsername,
            Amount = leaderboardProgress.Amount,
            Placement = leaderboardProgress.Placement
        };
    }
}