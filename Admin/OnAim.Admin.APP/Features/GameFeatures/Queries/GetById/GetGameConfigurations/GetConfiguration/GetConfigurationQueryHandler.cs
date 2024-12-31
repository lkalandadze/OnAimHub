using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations.GetConfiguration;

public class GetConfigurationQueryHandler : IQueryHandler<GetConfigurationQuery, ApplicationResult>
{
    private readonly IGameService _gameService;

    public GetConfigurationQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
    {
        var res = await _gameService.GetConfiguration(request.Name, request.Id);

        return new ApplicationResult { Data = res };
    }
}
