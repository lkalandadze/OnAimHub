using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActivePlayers;

public record GetActivePlayersBySegmentIdQuery(string SegmentId, FilterBy Filter) : IQuery<ApplicationResult>;

