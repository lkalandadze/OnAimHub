using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.DataModels;

public class LeaderboardSchedulesModel
{
    public int Id { get; set; }
    public string Name { get; set; } //Template name
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public DateOnly? SpecificDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardScheduleStatus Status { get; set; }

    public static LeaderboardSchedulesModel MapFrom(LeaderboardSchedule leaderboardSchedule)
    {
        return new LeaderboardSchedulesModel
        {
            Id = leaderboardSchedule.Id,
            Name = leaderboardSchedule.Name,
            RepeatType = leaderboardSchedule.RepeatType,
            RepeatValue = leaderboardSchedule.RepeatValue,
            SpecificDate = leaderboardSchedule.SpecificDate,
            StartDate = leaderboardSchedule.StartDate,
            EndDate = leaderboardSchedule.EndDate,
            Status = leaderboardSchedule.Status
        };
    }
}
