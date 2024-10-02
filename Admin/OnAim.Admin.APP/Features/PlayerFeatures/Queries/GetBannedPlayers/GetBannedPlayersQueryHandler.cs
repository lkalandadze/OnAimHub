using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayers;

public class GetBannedPlayersQueryHandler : IQueryHandler<GetBannedPlayersQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<PlayerBan> _readOnlyRepository;

    public GetBannedPlayersQueryHandler(IReadOnlyRepository<PlayerBan> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<ApplicationResult> Handle(GetBannedPlayersQuery request, CancellationToken cancellationToken)
    {
        var banned = _readOnlyRepository.Query();

        var result = banned.Select(x => new BannedPlayerListDto
        {
            PlayerId = x.PlayerId,
            PlayerName = x.Player.UserName,
            Description = x.Description,
            DateBanned = x.DateBanned,
            ExpireDate = x.ExpireDate,
            IsPermanent = x.IsPermanent,
            IsRevoked = x.IsRevoked,
            RevokeDate = x.RevokeDate,
        });


        return new ApplicationResult
        {
            Success = true,
            Data = await result.ToListAsync(cancellationToken)
        };
    }
}
