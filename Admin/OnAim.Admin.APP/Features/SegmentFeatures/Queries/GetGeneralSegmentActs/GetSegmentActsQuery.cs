using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetSegmentActs;

public record GetSegmentActsQuery(SegmentActsFilter Filter) : IQuery<ApplicationResult>;

