using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;

public sealed class GetAllSegmentQueryHandler : IQueryHandler<GetAllSegmentQuery, ApplicationResult<PaginatedResult<SegmentListDto>>>
{
    private readonly ISegmentService _segmentService;

    public GetAllSegmentQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }
    public async Task<ApplicationResult<PaginatedResult<SegmentListDto>>> Handle(GetAllSegmentQuery request, CancellationToken cancellationToken)
    {
        return await _segmentService.GetAll(request.PageNumber, request.PageSize);
    }
}
