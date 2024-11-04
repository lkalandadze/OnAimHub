using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllPrizes;

public class GetAllPrizesQueryHandler : IQueryHandler<GetAllPrizesQuery, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetAllPrizesQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(GetAllPrizesQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.GetAllPrizes();

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
