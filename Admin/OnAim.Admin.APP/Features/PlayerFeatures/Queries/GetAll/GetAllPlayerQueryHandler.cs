using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetAll;

public class GetAllPlayerQueryHandler : IQueryHandler<GetAllPlayerQuery, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public GetAllPlayerQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    } 
    public async Task<ApplicationResult> Handle(GetAllPlayerQuery request, CancellationToken cancellationToken)
    {
        var result = await _playerService.GetAll(request.Filter);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
