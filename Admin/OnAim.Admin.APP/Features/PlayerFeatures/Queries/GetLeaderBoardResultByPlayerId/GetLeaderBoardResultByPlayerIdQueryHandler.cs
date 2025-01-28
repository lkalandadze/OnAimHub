using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public class GetLeaderBoardResultByPlayerIdQueryHandler : IQueryHandler<GetLeaderBoardResultByPlayerIdQuery, ApplicationResult<List<LeaderboardResult>>>
{
    private readonly IPlayerService _playerService;

    public GetLeaderBoardResultByPlayerIdQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult<List<LeaderboardResult>>> Handle(GetLeaderBoardResultByPlayerIdQuery request, CancellationToken cancellationToken)
    {         
        return await _playerService.GetLeaderBoardResultByPlayer(request.PlayerId);
    }
}
