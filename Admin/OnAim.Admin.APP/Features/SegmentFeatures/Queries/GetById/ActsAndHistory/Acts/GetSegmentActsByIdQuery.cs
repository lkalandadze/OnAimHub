using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.Acts;

public record GetSegmentActsByIdQuery(string SegmentId) : IQuery<ApplicationResult<IEnumerable<ActsDto>>>;
