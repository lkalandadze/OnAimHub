using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetCalendar;

public class GetCalendarQueryHandler : IQueryHandler<GetCalendarQuery, object>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetCalendarQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<object> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.GetCalendar(request.StartDate, request.EndDate);
    }
}
