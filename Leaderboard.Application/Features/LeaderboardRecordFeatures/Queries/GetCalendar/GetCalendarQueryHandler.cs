using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Application.Services.Abstract;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;

public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, GetCalendarQueryResponse>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ICalendarService _calendarService;

    public GetCalendarQueryHandler(ILeaderboardRecordRepository leaderboardRecordRepository, ICalendarService calendarService)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _calendarService = calendarService;
    }

    public async Task<GetCalendarQueryResponse> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
    {
        var leaderboardRecords = _leaderboardRecordRepository.Query();


        if (request.StartDate.HasValue)
            leaderboardRecords = leaderboardRecords.Where(x => x.StartDate >= request.StartDate.Value.ToUniversalTime());

        if (request.EndDate.HasValue)
            leaderboardRecords = leaderboardRecords.Where(x => x.EndDate <= request.EndDate.Value.ToUniversalTime());

        var leaderboardRecordList = leaderboardRecords
            .Select(x => LeaderboardRecordsModel.MapFrom(x))
            .ToList();

        var futureLeaderboards = _calendarService.GenerateFutureLeaderboards(request);

        leaderboardRecordList.AddRange(futureLeaderboards);

        var response = new GetCalendarQueryResponse(leaderboardRecordList);

        return response;
    }
}