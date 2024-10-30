using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;

public record GetBlackListedPlayersBySegmentIdQuery(string SegmentId, FilterBy Filter) : IQuery<ApplicationResult>;

public record FilterBy(string? UserName): BaseFilter;
