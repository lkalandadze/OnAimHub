using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;
using Leaderboard.Application.Services.Abstract;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Services.Concrete;

public class CalendarService : ICalendarService
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    public CalendarService(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }
    public List<LeaderboardRecordsModel> GenerateFutureLeaderboards(GetCalendarQuery request)
    {
        var futureLeaderboards = new List<LeaderboardRecordsModel>();
        var now = DateTimeOffset.UtcNow;

        // Use EndDate parameter from the request for how far to generate future leaderboards
        var endDateForFuture = request.EndDate.HasValue ? request.EndDate.Value : now.AddMonths(1); // Default 1 month if not provided

        // Retrieve a single leaderboard template from the repository (adjust the query as needed)
        var template = _leaderboardTemplateRepository.Query().FirstOrDefault(); // Example: query the template you need

        // Ensure a template was found
        if (template == null)
        {
            throw new Exception("Leaderboard template not found.");
        }

        // Start generating based on the template's recurrence settings
        var nextStartDate = CalculateNextStartDate(now, template); // Dynamic start date based on template

        while (nextStartDate < endDateForFuture)
        {
            var endDate = nextStartDate.AddDays(template.DurationInDays); // Duration from template

            var futureLeaderboard = new LeaderboardRecordsModel
            {
                Id = 0, // No real ID since this is a dynamically generated record
                CreationDate = nextStartDate.AddDays(-template.CreationLeadTimeInDays), // Creation lead time from template
                AnnouncementDate = nextStartDate.AddDays(-template.AnnouncementLeadTimeInDays), // Announcement lead time from template
                StartDate = nextStartDate.ToUniversalTime(),
                EndDate = endDate.ToUniversalTime(),
                LeaderboardType = LeaderboardType.Win, // Use type from the template
                Status = LeaderboardRecordStatus.Future // Status for future leaderboards
            };

            futureLeaderboards.Add(futureLeaderboard);

            // Move to the next interval based on the JobType (weekly, monthly, custom) from the template
            nextStartDate = CalculateNextInterval(nextStartDate, template);
        }

        return futureLeaderboards;
    }


    public DateTimeOffset CalculateNextStartDate(DateTimeOffset current, LeaderboardTemplate template)
    {
        switch (template.JobType)
        {
            case JobTypeEnum.Daily:
                var todayStart = current.Date.Add(template.StartTime);
                return current >= todayStart ? todayStart.AddDays(1) : todayStart;
            case JobTypeEnum.Weekly:
                return current.Date.AddDays(-(int)current.DayOfWeek + (int)DayOfWeek.Monday); // Start next Monday
            case JobTypeEnum.Monthly:
                return new DateTimeOffset(current.Year, current.Month, 1, 0, 0, 0, TimeSpan.Zero).AddMonths(1); // Start on the 1st of the next month
            default:
                throw new ArgumentException("Unsupported job type");
        }
    }

    public DateTimeOffset CalculateNextInterval(DateTimeOffset startDate, LeaderboardTemplate template)
    {
        switch (template.JobType)
        {
            case JobTypeEnum.Daily:
                return startDate.AddDays(1);
            case JobTypeEnum.Weekly:
                return startDate.AddDays(7); // Move to the next week
            case JobTypeEnum.Monthly:
                return startDate.AddMonths(1); // Move to the next month
            default:
                throw new ArgumentException("Unsupported job type");
        }
    }
}
