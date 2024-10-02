using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetProgress;

public class GetPlayerProgressQueryHandler : IQueryHandler<GetPlayerProgressQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<PlayerProgress> _readOnlyRepository;

    public GetPlayerProgressQueryHandler(IReadOnlyRepository<PlayerProgress> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<ApplicationResult> Handle(GetPlayerProgressQuery request, CancellationToken cancellationToken)
    {
        var progress = await _readOnlyRepository.Query(x => x.PlayerId == request.Id).FirstOrDefaultAsync();

        var result = new PlayerProgressDto
        {
            DailyProgress = progress.Progress,
            TotalProgress = progress.Progress
        };

        return new ApplicationResult
        {
            Success = true,
            Data = result
        };
    }
}
