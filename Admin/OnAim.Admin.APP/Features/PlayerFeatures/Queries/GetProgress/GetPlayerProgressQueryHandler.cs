using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetProgress;

public class GetPlayerProgressQueryHandler : IQueryHandler<GetPlayerProgressQuery, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public GetPlayerProgressQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult> Handle(GetPlayerProgressQuery request, CancellationToken cancellationToken)
    {
        var result = await _playerService.GetPlayerProgress(request.Id);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
