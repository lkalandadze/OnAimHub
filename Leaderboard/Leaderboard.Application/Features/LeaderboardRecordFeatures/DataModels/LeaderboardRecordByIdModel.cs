using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;

public sealed record LeaderboardRecordByIdModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }
    public List<LeaderboardPrizeModel> Prizes { get; set; } = new();

    public static LeaderboardRecordByIdModel MapFrom(LeaderboardRecord leaderboardRecord)
    {
        return new LeaderboardRecordByIdModel
        {
            Id = leaderboardRecord.Id,
            Name = leaderboardRecord.Name,
            Description = leaderboardRecord.Description,
            CreationDate = leaderboardRecord.CreationDate,
            AnnouncementDate = leaderboardRecord.AnnouncementDate,
            StartDate = leaderboardRecord.StartDate,
            EndDate = leaderboardRecord.EndDate,
            LeaderboardType = leaderboardRecord.LeaderboardType,
            Prizes = leaderboardRecord.LeaderboardRecordPrizes?.Select(prize => new LeaderboardPrizeModel
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