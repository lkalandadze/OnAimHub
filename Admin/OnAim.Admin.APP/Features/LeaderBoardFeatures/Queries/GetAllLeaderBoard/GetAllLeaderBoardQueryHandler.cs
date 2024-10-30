using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;

public class GetAllLeaderBoardQueryHandler : IQueryHandler<GetAllLeaderBoardQuery, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetAllLeaderBoardQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(GetAllLeaderBoardQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.GetAllLeaderBoard(request.Filter);

        return new ApplicationResult { Data = result.Data , Success = result.Success };
    }
}
