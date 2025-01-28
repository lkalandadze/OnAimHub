using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.Acts;

public class GetSegmentActsByIdQueryHandler : IQueryHandler<GetSegmentActsByIdQuery, ApplicationResult<IEnumerable<ActsDto>>>
{
    private readonly ISegmentService _segmentService;

    public GetSegmentActsByIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }
    public async Task<ApplicationResult<IEnumerable<ActsDto>>> Handle(GetSegmentActsByIdQuery request, CancellationToken cancellationToken)
    {
        return await _segmentService.GetActs(request.SegmentId);
    }
}
