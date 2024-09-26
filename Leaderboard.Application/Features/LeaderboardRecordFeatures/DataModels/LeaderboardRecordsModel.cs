using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;

public sealed record LeaderboardRecordsModel
{
    public int Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }


    public static LeaderboardRecordsModel MapFrom(LeaderboardRecord leaderboardRecord)
    {
        return new LeaderboardRecordsModel
        {
            Id = leaderboardRecord.Id,
            CreationDate = leaderboardRecord.CreationDate,
            AnnouncementDate = leaderboardRecord.AnnouncementDate,
            StartDate = leaderboardRecord.StartDate,
            EndDate = leaderboardRecord.EndDate,
            LeaderboardType = leaderboardRecord.LeaderboardType
        };
    }
}