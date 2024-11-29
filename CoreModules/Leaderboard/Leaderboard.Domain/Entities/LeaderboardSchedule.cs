using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardSchedule : BaseEntity<int>
{
    public LeaderboardSchedule(string title, RepeatType repeatType, int? repeatValue, int leaderboardRecordId)
    {
        Title = title;
        RepeatType = repeatType;
        RepeatValue = repeatValue;
        LeaderboardRecordId = leaderboardRecordId;
        Status = LeaderboardScheduleStatus.Active;
    }

    public string Title { get; set; }  //R
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public LeaderboardScheduleStatus Status { get; set; } //R? Need logic if we need it
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }


    //SingleDate: RepeatValue might be unused in this case, as you would rely on StartDate to determine the job's execution date.

    //EveryNDays: RepeatValue stores the interval in days(e.g., 3 for every 3 days).

    //DayOfWeek: RepeatValue could store an integer from 0 to 6 representing the day of the week(e.g., 0 for Sunday, 1 for Monday).

    //DayOfMonth: RepeatValue could store an integer from 1 to 31, representing the day of the month.

    public void UpdateStatus(LeaderboardScheduleStatus status)
    {
        Status = status;
    }
}
