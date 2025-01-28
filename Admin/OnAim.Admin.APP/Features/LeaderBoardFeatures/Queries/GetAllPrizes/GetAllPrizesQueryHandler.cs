using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllPrizes;

public class GetAllPrizesQueryHandler : IQueryHandler<GetAllPrizesQuery, object>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetAllPrizesQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<object> Handle(GetAllPrizesQuery request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.GetAllPrizes();
    }
}
