using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.History;

public class GetSegmentActsHistoryByIdQueryHandler : IQueryHandler<GetSegmentActsHistoryByIdQuery, ApplicationResult>
{
    private readonly ISegmentService _segmentService;

    public GetSegmentActsHistoryByIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }
    public async Task<ApplicationResult> Handle(GetSegmentActsHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _segmentService.GetActsHistory(request.PlayerSegmentActId);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
