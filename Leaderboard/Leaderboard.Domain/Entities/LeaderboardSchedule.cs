using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardSchedule : BaseEntity<int>
{
    public LeaderboardSchedule(string name, RepeatType repeatType, int? repeatValue, DateTimeOffset startDate, DateTimeOffset endDate, int leaderboardTemplateId)
    {
        Name = name;
        RepeatType = repeatType;
        RepeatValue = repeatValue;
        StartDate = startDate;
        EndDate = endDate;
        LeaderboardTemplateId = leaderboardTemplateId;
        Status = LeaderboardScheduleStatus.Active;
    }

    public string Name { get; set; } //Template name
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public DateOnly? SpecificDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardScheduleStatus Status { get; set; }
    public int LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }


    //SingleDate: RepeatValue might be unused in this case, as you would rely on StartDate to determine the job's execution date.

    //EveryNDays: RepeatValue stores the interval in days(e.g., 3 for every 3 days).

    //DayOfWeek: RepeatValue could store an integer from 0 to 6 representing the day of the week(e.g., 0 for Sunday, 1 for Monday).

    //DayOfMonth: RepeatValue could store an integer from 1 to 31, representing the day of the month.

    public void UpdateStatus(LeaderboardScheduleStatus status)
    {
        Status = status;
    }
}
