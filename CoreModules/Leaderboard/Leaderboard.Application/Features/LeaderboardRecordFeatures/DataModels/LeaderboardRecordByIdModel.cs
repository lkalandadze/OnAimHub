using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;

public sealed record LeaderboardRecordByIdModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<LeaderboardPrizeModel> Prizes { get; set; } = new();

    public static LeaderboardRecordByIdModel MapFrom(LeaderboardRecord leaderboardRecord)
    {
        return new LeaderboardRecordByIdModel
        {
            Id = leaderboardRecord.Id,
            Title = leaderboardRecord.Title,
            Description = leaderboardRecord.Description,
            EventType = leaderboardRecord.EventType,
            CreationDate = leaderboardRecord.CreationDate,
            AnnouncementDate = leaderboardRecord.AnnouncementDate,
            StartDate = leaderboardRecord.StartDate,
            EndDate = leaderboardRecord.EndDate,
            Prizes = leaderboardRecord.LeaderboardRecordPrizes?.Select(prize => new LeaderboardPrizeModel
            {
                Id = prize.Id,
                StartRank = prize.StartRank,
                EndRank = prize.EndRank,
                CoinId = prize.CoinId,
                Amount = prize.Amount
            }).ToList() ?? new List<LeaderboardPrizeModel>()
        };
    }
}