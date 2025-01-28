using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetProgress;

public class GetPlayerProgressQueryHandler : IQueryHandler<GetPlayerProgressQuery, ApplicationResult<PlayerProgressDto>>
{
    private readonly IPlayerService _playerService;

    public GetPlayerProgressQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult<PlayerProgressDto>> Handle(GetPlayerProgressQuery request, CancellationToken cancellationToken)
    {
        return await _playerService.GetPlayerProgress(request.Id);
    }
}
