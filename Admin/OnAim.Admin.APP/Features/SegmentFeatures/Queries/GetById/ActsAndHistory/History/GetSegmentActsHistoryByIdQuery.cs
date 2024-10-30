using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.History;

public record GetSegmentActsHistoryByIdQuery(int PlayerSegmentActId) : IQuery<ApplicationResult>;
