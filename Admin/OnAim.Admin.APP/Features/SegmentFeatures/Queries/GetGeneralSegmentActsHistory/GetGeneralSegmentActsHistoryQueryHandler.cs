using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetGeneralSegmentActsHistory;

public class GetGeneralSegmentActsHistoryQueryHandler : IQueryHandler<GetGeneralSegmentActsHistoryQuery, ApplicationResult>
{
    private readonly ISegmentService _segmentService;

    public GetGeneralSegmentActsHistoryQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult> Handle(GetGeneralSegmentActsHistoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _segmentService.GetGeneralSegmentActsHistory(request.Filter);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
