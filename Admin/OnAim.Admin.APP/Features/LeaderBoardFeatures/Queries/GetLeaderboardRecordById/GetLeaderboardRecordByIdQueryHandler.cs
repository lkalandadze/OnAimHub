using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardRecordById;

public class GetLeaderboardRecordByIdQueryHandler : IQueryHandler<GetLeaderboardRecordByIdQuery, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetLeaderboardRecordByIdQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(GetLeaderboardRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.GetLeaderboardRecordById(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
