using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayer;

public record GetBannedPlayerQuery(int Id) : IQuery<ApplicationResult<PlayerBan>>;
