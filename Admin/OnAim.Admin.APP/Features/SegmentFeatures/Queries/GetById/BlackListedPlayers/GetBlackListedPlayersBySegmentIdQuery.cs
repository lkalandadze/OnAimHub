using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;

public record GetBlackListedPlayersBySegmentIdQuery(string SegmentId, FilterBy Filter) : IQuery<ApplicationResult<PaginatedResult<SegmentPlayerDto>>>;

public class FilterBy : BaseFilter { public string? UserName { get; set; } }
