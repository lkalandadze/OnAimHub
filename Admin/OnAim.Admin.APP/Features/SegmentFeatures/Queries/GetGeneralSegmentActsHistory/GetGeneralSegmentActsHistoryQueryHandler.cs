using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetGeneralSegmentActsHistory;

public class GetGeneralSegmentActsHistoryQueryHandler : IQueryHandler<GetGeneralSegmentActsHistoryQuery, ApplicationResult<PaginatedResult<ActsHistoryDto>>>
{
    private readonly ISegmentService _segmentService;

    public GetGeneralSegmentActsHistoryQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult<PaginatedResult<ActsHistoryDto>>> Handle(GetGeneralSegmentActsHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _segmentService.GetGeneralSegmentActsHistory(request.Filter);
    }
}
