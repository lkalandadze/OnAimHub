using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations;

public class GetGameConfigurationsQueryHandler : IQueryHandler<GetGameConfigurationsQuery, ApplicationResult>
{
    private readonly IGameService _gameService;

    public GetGameConfigurationsQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(GetGameConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var res = await _gameService.GetConfigurations();

        return new ApplicationResult { Data = res };
    }
}
