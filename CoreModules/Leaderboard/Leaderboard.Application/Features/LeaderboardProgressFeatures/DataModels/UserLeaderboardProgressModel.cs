using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;

public sealed record UserLeaderboardProgressModel
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int Placement { get; set; }
    public int Score { get; set; }
    public string? CoinId { get; set; }
    public int? PrizeAmount { get; set; }
    public int PromotionId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public static UserLeaderboardProgressModel MapFrom(
        LeaderboardProgress leaderboardProgress,
        int placement,
        int score,
        string? coinId = null,
        int? prizeAmount = null,
        int promotionId = default,
        DateTimeOffset startDate = default,
        DateTimeOffset endDate = default)
    {
        return new UserLeaderboardProgressModel
        {
            Id = leaderboardProgress.LeaderboardRecordId,
            Amount = leaderboardProgress.Amount,
            Placement = placement,
            Score = score,
            CoinId = coinId,
            PrizeAmount = prizeAmount,
            PromotionId = promotionId,
            StartDate = startDate,
            EndDate = endDate
        };
    }
}