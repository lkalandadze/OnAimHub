using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.History;

public record GetSegmentActsHistoryByIdQuery(int PlayerSegmentActId) : IQuery<ApplicationResult<IEnumerable<ActsHistoryDto>>>;
