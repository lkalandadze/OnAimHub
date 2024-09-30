using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;
using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Services.Abstract;

public interface ICalendarService
{
    List<LeaderboardRecordsModel> GenerateFutureLeaderboards(GetCalendarQuery request);
    DateTimeOffset CalculateNextStartDate(DateTimeOffset current, LeaderboardTemplate template);
    DateTimeOffset CalculateNextInterval(DateTimeOffset startDate, LeaderboardTemplate template);
}
