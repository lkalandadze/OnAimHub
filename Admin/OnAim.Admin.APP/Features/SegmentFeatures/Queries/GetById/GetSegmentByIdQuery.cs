using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById;

public sealed record GetSegmentByIdQuery(string SegmentId) : IQuery<ApplicationResult>;
