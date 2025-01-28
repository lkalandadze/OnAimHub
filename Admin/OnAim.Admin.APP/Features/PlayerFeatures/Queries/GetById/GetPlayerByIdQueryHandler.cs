using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetById;

public class GetPlayerByIdQueryHandler : IQueryHandler<GetPlayerByIdQuery, ApplicationResult<PlayerDto>>
{
    private readonly IPlayerService _playerService;

    public GetPlayerByIdQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task<ApplicationResult<PlayerDto>> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
    {    
        return await _playerService.GetById(request.Id);
    }

}
