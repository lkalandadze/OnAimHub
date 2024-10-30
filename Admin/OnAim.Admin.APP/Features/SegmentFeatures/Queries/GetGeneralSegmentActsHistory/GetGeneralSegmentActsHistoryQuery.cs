using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetGeneralSegmentActsHistory;

public record GetGeneralSegmentActsHistoryQuery(SegmentActsFilter Filter) : IQuery<ApplicationResult>;
