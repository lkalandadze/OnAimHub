using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardSchedules;

public class GetLeaderboardSchedulesQueryHandler : IQueryHandler<GetLeaderboardSchedulesQuery, object>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetLeaderboardSchedulesQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<object> Handle(GetLeaderboardSchedulesQuery request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.GetLeaderboardSchedules(request.PageNumber, request.PageSize);
    }
}
