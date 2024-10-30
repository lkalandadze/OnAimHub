using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetById;

public class GetPlayerByIdQueryHandler : IQueryHandler<GetPlayerByIdQuery, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public GetPlayerByIdQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task<ApplicationResult> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
    {    
        var result = await _playerService.GetById(request.Id);

        return new ApplicationResult
        {
            Data = result.Data,
            Success = result.Success,
        };
    }

}
