using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayers;

public record GetBannedPlayersQuery() : IQuery<ApplicationResult>;
