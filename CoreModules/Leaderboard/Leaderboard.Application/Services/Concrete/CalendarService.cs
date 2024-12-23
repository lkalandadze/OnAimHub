using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;
using Leaderboard.Application.Services.Abstract;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using Microsoft.EntityFrameworkCore;

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

        // Determine the end date for future leaderboards
        var endDateForFuture = request.EndDate ?? now.AddMonths(1); // Default to 1 month ahead if no end date provided

        // Retrieve the relevant leaderboard schedules
        var schedules = _leaderboardScheduleRepository.Query()
            .Include(s => s.LeaderboardRecord)
            .Where(s => s.Status != LeaderboardScheduleStatus.Cancelled)
            .ToList();

        if (!schedules.Any())
        {
            throw new Exception("No active leaderboard schedules found.");
        }

        foreach (var schedule in schedules)
        {
            var leaderboardRecord = schedule.LeaderboardRecord;
            if (leaderboardRecord == null)
            {
                throw new Exception($"No associated leaderboard record found for schedule ID {schedule.Id}.");
            }

            // Calculate the next start date based on the schedule
            var nextStartDate = CalculateNextStartDate(now, schedule.LeaderboardRecord, schedule);

            // Generate future leaderboards within the date range
            while (nextStartDate < endDateForFuture)
            {
                var endDate = nextStartDate.AddDays((leaderboardRecord.EndDate - leaderboardRecord.StartDate).Days);

                var futureLeaderboard = new LeaderboardRecordsModel
                {
                    Id = 0, // Placeholder ID for dynamically generated leaderboard
                    PromotionId = leaderboardRecord.PromotionId,
                    PromotionName = leaderboardRecord.PromotionName,
                    Title = leaderboardRecord.Title,
                    Description = leaderboardRecord.Description,
                    CreationDate = nextStartDate.AddDays(-1), // Placeholder: 1 day before the start date
                    AnnouncementDate = nextStartDate.AddDays(-leaderboardRecord.StartDate.Subtract(leaderboardRecord.AnnouncementDate).Days),
                    StartDate = nextStartDate.ToUniversalTime(),
                    EndDate = endDate.ToUniversalTime(),
                    Status = LeaderboardRecordStatus.Future // Mark as future
                };

                futureLeaderboards.Add(futureLeaderboard);

                // Move to the next interval
                nextStartDate = CalculateNextInterval(nextStartDate, schedule);
            }
        }

        return futureLeaderboards;
    }

    public DateTimeOffset CalculateNextStartDate(DateTimeOffset current, LeaderboardRecord record, LeaderboardSchedule schedule)
    {
        var startTime = record.StartDate.TimeOfDay;

        return schedule.RepeatType switch
        {
            RepeatType.EveryNDays => current.Date.AddDays(schedule.RepeatValue ?? 1).Add(startTime),

            RepeatType.DayOfWeek => current.Date.AddDays(((schedule.RepeatValue ?? 0) - (int)current.DayOfWeek + 7) % 7).Add(startTime),

            RepeatType.DayOfMonth =>
                current.Day <= (schedule.RepeatValue ?? 1)
                    ? new DateTimeOffset(current.Year, current.Month, schedule.RepeatValue ?? 1, startTime.Hours, startTime.Minutes, startTime.Seconds, current.Offset)
                    : new DateTimeOffset(current.AddMonths(1).Year, current.AddMonths(1).Month, schedule.RepeatValue ?? 1, startTime.Hours, startTime.Minutes, startTime.Seconds, current.Offset),

            _ => throw new ArgumentException("Unsupported repeat type")
        };
    }

    public DateTimeOffset CalculateNextInterval(DateTimeOffset startDate, LeaderboardSchedule schedule)
    {
        return schedule.RepeatType switch
        {
            RepeatType.None => DateTimeOffset.MaxValue, // No recurrence for SingleDate

            RepeatType.EveryNDays => startDate.AddDays(schedule.RepeatValue ?? 1),

            RepeatType.DayOfWeek => startDate.AddDays(7), // Move to the same day next week

            RepeatType.DayOfMonth => startDate.AddMonths(1), // Move to the same day next month

            _ => throw new ArgumentException("Unsupported repeat type")
        };
    }
}
