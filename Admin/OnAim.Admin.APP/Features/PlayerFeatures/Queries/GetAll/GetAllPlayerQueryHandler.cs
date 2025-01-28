using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetAll;

public class GetAllPlayerQueryHandler : IQueryHandler<GetAllPlayerQuery, ApplicationResult<PaginatedResult<PlayerListDto>>>
{
    private readonly IPlayerService _playerService;

    public GetAllPlayerQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    } 
    public async Task<ApplicationResult<PaginatedResult<PlayerListDto>>> Handle(GetAllPlayerQuery request, CancellationToken cancellationToken)
    {
        return await _playerService.GetAll(request.Filter);
    }
}
