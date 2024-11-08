using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;
using Leaderboard.Application.Services.Abstract;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaderboard.Application.Services.Concrete;

public class CalendarService : ICalendarService
{
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;

    public CalendarService(ILeaderboardScheduleRepository leaderboardScheduleRepository)
    {
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
    }

    public List<LeaderboardRecordsModel> GenerateFutureLeaderboards(GetCalendarQuery request)
    {
        var futureLeaderboards = new List<LeaderboardRecordsModel>();
        var now = DateTimeOffset.UtcNow;

        // Use EndDate parameter from the request for how far to generate future leaderboards
        var endDateForFuture = request.EndDate.HasValue ? request.EndDate.Value : now.AddMonths(1); // Default 1 month if not provided

        // Retrieve the relevant leaderboard schedule from the repository
        var schedule = _leaderboardScheduleRepository.Query()
            .Include(s => s.LeaderboardTemplate)
            .FirstOrDefault(x => x.Status != LeaderboardScheduleStatus.Cancelled); // Adjust to get the appropriate schedule based on request

        // Ensure a schedule was found
        if (schedule == default)
        {
            throw new Exception("Leaderboard schedule not found.");
        }

        var leaderboardTemplate = schedule.LeaderboardTemplate;
        if (leaderboardTemplate == null)
        {
            throw new Exception("Associated leaderboard template not found.");
        }

        // Start generating based on the schedule's recurrence settings
        var nextStartDate = CalculateNextStartDate(now, schedule); // Dynamic start date based on schedule

        while (nextStartDate < endDateForFuture)
        {
            var endDate = nextStartDate.AddDays(leaderboardTemplate.EndIn); // Duration from template

            var futureLeaderboard = new LeaderboardRecordsModel
            {
                Id = 0, // No real ID since this is a dynamically generated record
                CreationDate = nextStartDate.AddDays(-leaderboardTemplate.AnnounceIn), // Creation lead time from template
                AnnouncementDate = nextStartDate.AddDays(-leaderboardTemplate.StartIn), // Announcement lead time from template
                StartDate = nextStartDate.ToUniversalTime(),
                EndDate = endDate.ToUniversalTime(),
                LeaderboardType = LeaderboardType.Win, // Use type from the template
                Status = LeaderboardRecordStatus.Future // Status for future leaderboards
            };

            futureLeaderboards.Add(futureLeaderboard);

            // Move to the next interval based on RepeatType and RepeatValue from the schedule
            nextStartDate = CalculateNextInterval(nextStartDate, schedule);
        }

        return futureLeaderboards;
    }

    public DateTimeOffset CalculateNextStartDate(DateTimeOffset current, LeaderboardSchedule schedule)
    {
        var startTime = schedule.LeaderboardTemplate?.StartTime ?? TimeSpan.Zero;

        switch (schedule.RepeatType)
        {
            case RepeatType.SingleDate:
                if (!schedule.SpecificDate.HasValue)
                    throw new Exception("SpecificDate is required for RepeatType.SingleDate");
                var specificDateTime = new DateTime(schedule.SpecificDate.Value.Year, schedule.SpecificDate.Value.Month, schedule.SpecificDate.Value.Day, startTime.Hours, startTime.Minutes, startTime.Seconds);
                return specificDateTime > current ? new DateTimeOffset(specificDateTime, TimeSpan.Zero) : specificDateTime.AddYears(1); // Run once or move to next year

            case RepeatType.EveryNDays:
                return current.Date.AddDays(schedule.RepeatValue ?? 1).Add(startTime);

            case RepeatType.DayOfWeek:
                var daysUntilTargetDay = ((schedule.RepeatValue.Value - (int)current.DayOfWeek + 7) % 7);
                return current.Date.AddDays(daysUntilTargetDay).Add(startTime);

            case RepeatType.DayOfMonth:
                var targetDay = schedule.RepeatValue ?? 1;
                var currentMonth = new DateTime(current.Year, current.Month, 1);
                var nextMonth = currentMonth.AddMonths(1);
                var nextScheduledDate = new DateTime(nextMonth.Year, nextMonth.Month, targetDay);
                return new DateTimeOffset(nextScheduledDate.Add(startTime), TimeSpan.Zero);

            default:
                throw new ArgumentException("Unsupported repeat type");
        }
    }

    public DateTimeOffset CalculateNextInterval(DateTimeOffset startDate, LeaderboardSchedule schedule)
    {
        switch (schedule.RepeatType)
        {
            case RepeatType.SingleDate:
                return DateTimeOffset.MaxValue; // Only one occurrence; no next interval

            case RepeatType.EveryNDays:
                return startDate.AddDays(schedule.RepeatValue ?? 1);

            case RepeatType.DayOfWeek:
                return startDate.AddDays(7); // Move to the next week

            case RepeatType.DayOfMonth:
                return startDate.AddMonths(1); // Move to the next month

            default:
                throw new ArgumentException("Unsupported repeat type");
        }
    }
}
