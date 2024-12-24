using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;

public record GetBlackListedPlayersBySegmentIdQuery(string SegmentId, FilterBy Filter) : IQuery<ApplicationResult>;

public class FilterBy : BaseFilter { public string? UserName { get; set; } }
