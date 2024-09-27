using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;

public class GetCalendarQueryResponse : Response<List<LeaderboardRecordsModel>>
{
    public GetCalendarQueryResponse() : base()
    {
    }

    public GetCalendarQueryResponse(List<LeaderboardRecordsModel> data, string message = null) : base(data, message)
    {
    }
}