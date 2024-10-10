using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;

public sealed class GetAllSegmentQueryHandler : IQueryHandler<GetAllSegmentQuery, ApplicationResult>
{
    private readonly ISegmentService _segmentService;

    public GetAllSegmentQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }
    public async Task<ApplicationResult> Handle(GetAllSegmentQuery request, CancellationToken cancellationToken)
    {
        var result = await _segmentService.GetAll(request.PageNumber, request.PageSize);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
