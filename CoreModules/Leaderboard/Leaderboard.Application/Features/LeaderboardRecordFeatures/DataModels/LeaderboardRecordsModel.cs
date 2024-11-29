using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;

public sealed record LeaderboardRecordsModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }


    public static LeaderboardRecordsModel MapFrom(LeaderboardRecord leaderboardRecord)
    {
        return new LeaderboardRecordsModel
        {
            Id = leaderboardRecord.Id,
            Title = leaderboardRecord.Title,
            Description = leaderboardRecord.Description,
            EventType = leaderboardRecord.EventType,
            CreationDate = leaderboardRecord.CreationDate,
            AnnouncementDate = leaderboardRecord.AnnouncementDate,
            StartDate = leaderboardRecord.StartDate,
            EndDate = leaderboardRecord.EndDate,
            Status = leaderboardRecord.Status,
            IsGenerated = leaderboardRecord.IsGenerated
        };
    }
}