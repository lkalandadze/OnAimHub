using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById;

public sealed class GetSegmentByIdQueryHandler : IQueryHandler<GetSegmentByIdQuery, ApplicationResult>
{
    private readonly ISegmentService _segmentService;

    public GetSegmentByIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult> Handle(GetSegmentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _segmentService.GetById(request.SegmentId);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
