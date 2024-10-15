using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.Acts;

public class GetSegmentActsByIdQueryHandler : IQueryHandler<GetSegmentActsByIdQuery, ApplicationResult>
{
    private readonly ISegmentService _segmentService;

    public GetSegmentActsByIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }
    public async Task<ApplicationResult> Handle(GetSegmentActsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _segmentService.GetActs(request.SegmentId);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
