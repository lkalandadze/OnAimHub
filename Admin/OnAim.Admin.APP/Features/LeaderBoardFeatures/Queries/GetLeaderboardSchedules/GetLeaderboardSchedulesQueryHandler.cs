using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardSchedules;

public class GetLeaderboardSchedulesQueryHandler : IQueryHandler<GetLeaderboardSchedulesQuery, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetLeaderboardSchedulesQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(GetLeaderboardSchedulesQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.GetLeaderboardSchedules(request.PageNumber, request.PageSize);

        return new ApplicationResult
        {
            Data = result.Data,
            Success = result.Success,
        };
    }
}
