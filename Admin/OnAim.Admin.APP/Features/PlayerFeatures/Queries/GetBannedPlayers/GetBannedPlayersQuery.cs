using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayers;

public record GetBannedPlayersQuery() : IQuery<ApplicationResult<List<BannedPlayerListDto>>>;
