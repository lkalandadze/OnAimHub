using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.DataModels;

public class LeaderboardSchedulesModel
{
    public int Id { get; set; }
    public string Title { get; set; } //Template name
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public LeaderboardScheduleStatus Status { get; set; }

    public static LeaderboardSchedulesModel MapFrom(LeaderboardSchedule leaderboardSchedule)
    {
        return new LeaderboardSchedulesModel
        {
            Id = leaderboardSchedule.Id,
            Title = leaderboardSchedule.Title,
            RepeatType = leaderboardSchedule.RepeatType,
            RepeatValue = leaderboardSchedule.RepeatValue,
            Status = leaderboardSchedule.Status
        };
    }
}
