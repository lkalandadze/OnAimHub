using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActivePlayers;

public record GetActivePlayersBySegmentIdQuery(string SegmentId, FilterBy Filter) : IQuery<ApplicationResult<PaginatedResult<SegmentPlayerDto>>>;

