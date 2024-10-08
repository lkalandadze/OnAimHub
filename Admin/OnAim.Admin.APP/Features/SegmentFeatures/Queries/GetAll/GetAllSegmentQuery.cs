using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;

public sealed record GetAllSegmentQuery(int? PageNumber, int? PageSize) : IQuery<ApplicationResult>;
