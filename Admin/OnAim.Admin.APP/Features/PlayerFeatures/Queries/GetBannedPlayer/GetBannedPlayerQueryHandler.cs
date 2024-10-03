using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayer;

public class GetBannedPlayerQueryHandler : IQueryHandler<GetBannedPlayerQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<PlayerBan> _readOnlyRepository;

    public GetBannedPlayerQueryHandler(IReadOnlyRepository<PlayerBan> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<ApplicationResult> Handle(GetBannedPlayerQuery request, CancellationToken cancellationToken)
    {
        var palyer = await _readOnlyRepository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        return new ApplicationResult
        {
            Success = true,
            Data = palyer ?? null,
        };
    }
}
