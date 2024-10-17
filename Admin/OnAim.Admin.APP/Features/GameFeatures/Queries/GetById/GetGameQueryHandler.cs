using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;

public class GetGameQueryHandler : IQueryHandler<GetGameQuery, ApplicationResult>
{
    private readonly IGameService _gameService;

    public GetGameQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
