using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetConfigurationMetadata;

public class GetConfigurationMetadataQueryHandler : IQueryHandler<GetConfigurationMetadataQuery, ApplicationResult>
{
    private readonly IGameService _gameService;

    public GetConfigurationMetadataQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(GetConfigurationMetadataQuery request, CancellationToken cancellationToken)
    {
        var result = await _gameService.GetConfigurationMetadata();

        return new ApplicationResult { Data = result };
    }
}
