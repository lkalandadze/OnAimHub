using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardSchedule : BaseEntity<int>
{
    public LeaderboardSchedule(
        string title,
        RepeatType repeatType,
        int? repeatValue,
        int leaderboardRecordId,
        int announcementDurationHours,
        int startDurationHours,
        int endDurationHours)
    {
        Title = title;
        RepeatType = repeatType;
        RepeatValue = repeatValue;
        LeaderboardRecordId = leaderboardRecordId;
        AnnouncementDurationHours = announcementDurationHours;
        StartDurationHours = startDurationHours;
        EndDurationHours = endDurationHours;
        Status = LeaderboardScheduleStatus.Active;
    }


    public string Title { get; set; }  //R
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public LeaderboardScheduleStatus Status { get; set; } //R? Need logic if we need it
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }

    public int CreationHours { get; set; }
    public int AnnouncementDurationHours { get; set; }
    public int StartDurationHours { get; set; }
    public int EndDurationHours { get; set; }


    //SingleDate: RepeatValue might be unused in this case, as you would rely on StartDate to determine the job's execution date.

    //EveryNDays: RepeatValue stores the interval in days(e.g., 3 for every 3 days).

    //DayOfWeek: RepeatValue could store an integer from 0 to 6 representing the day of the week(e.g., 0 for Sunday, 1 for Monday).

    //DayOfMonth: RepeatValue could store an integer from 1 to 31, representing the day of the month.

    public void UpdateStatus(LeaderboardScheduleStatus status)
    {
        Status = status;
    }
    public DateTimeOffset CalculateAnnouncementDate(DateTimeOffset creationDate)
        => AddHoursWithValidation(creationDate, AnnouncementDurationHours);

    public DateTimeOffset CalculateStartDate(DateTimeOffset creationDate)
        => AddHoursWithValidation(creationDate, StartDurationHours);

    public DateTimeOffset CalculateEndDate(DateTimeOffset creationDate)
        => AddHoursWithValidation(creationDate, EndDurationHours);

    private DateTimeOffset AddHoursWithValidation(DateTimeOffset date, int hours)
    {
        try
        {
            // Check if adding the hours will exceed the allowable range
            if (hours > 0 && date + TimeSpan.FromHours(hours) > DateTimeOffset.MaxValue)
            {
                throw new InvalidOperationException($"The calculated date exceeds the maximum allowable range. Base date: {date}, hours to add: {hours}");
            }
            if (hours < 0 && date + TimeSpan.FromHours(hours) < DateTimeOffset.MinValue)
            {
                throw new InvalidOperationException($"The calculated date exceeds the minimum allowable range. Base date: {date}, hours to subtract: {hours}");
            }

            return date.AddHours(hours);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidOperationException($"The calculated date exceeds the allowable range. Base date: {date}, hours: {hours}");
        }
    }

    public (DateTimeOffset NextAnnouncementDate, DateTimeOffset NextStartDate, DateTimeOffset NextEndDate) CalculateNextDates(DateTimeOffset creationDate)
    {
        var announcementDate = CalculateAnnouncementDate(creationDate);
        var startDate = CalculateStartDate(creationDate);
        var endDate = CalculateEndDate(creationDate);

        return (announcementDate, startDate, endDate);
    }
}
