using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetCalendar;

public class GetCalendarQueryHandler : IQueryHandler<GetCalendarQuery, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetCalendarQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.GetCalendar(request.StartDate, request.EndDate);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
