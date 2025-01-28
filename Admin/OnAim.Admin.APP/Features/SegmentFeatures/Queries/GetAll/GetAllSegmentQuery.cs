using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;

public sealed record GetAllSegmentQuery(int? PageNumber, int? PageSize) : IQuery<ApplicationResult<PaginatedResult<SegmentListDto>>>;
