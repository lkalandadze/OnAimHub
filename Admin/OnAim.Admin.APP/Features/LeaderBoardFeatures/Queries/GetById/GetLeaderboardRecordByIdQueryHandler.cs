using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoard;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardRecordById;

public class GetLeaderboardRecordByIdQueryHandler : IQueryHandler<GetLeaderboardRecordByIdQuery, ApplicationResult<LeaderBoardData>>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetLeaderboardRecordByIdQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult<LeaderBoardData>> Handle(GetLeaderboardRecordByIdQuery request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.GetLeaderboardRecordById(request.Id);
    }
}
