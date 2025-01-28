using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.History;

public class GetSegmentActsHistoryByIdQueryHandler : IQueryHandler<GetSegmentActsHistoryByIdQuery, ApplicationResult<IEnumerable<ActsHistoryDto>>>
{
    private readonly ISegmentService _segmentService;

    public GetSegmentActsHistoryByIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }
    public async Task<ApplicationResult<IEnumerable<ActsHistoryDto>>> Handle(GetSegmentActsHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _segmentService.GetActsHistory(request.PlayerSegmentActId);
    }
}
