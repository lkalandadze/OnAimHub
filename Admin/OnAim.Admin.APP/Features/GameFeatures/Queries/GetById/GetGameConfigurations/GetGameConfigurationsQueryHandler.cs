using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations;

public class GetGameConfigurationsQueryHandler : IQueryHandler<GetGameConfigurationsQuery, object>
{
    private readonly IGameService _gameService;

    public GetGameConfigurationsQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(GetGameConfigurationsQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetConfigurations(request.Name, request.PromotionId, request.ConfigurationId);
    }
}
