using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetAll;

public sealed record GetAllPlayerQuery(PlayerFilter? Filter) : IQuery<ApplicationResult<PaginatedResult<PlayerListDto>>>;
