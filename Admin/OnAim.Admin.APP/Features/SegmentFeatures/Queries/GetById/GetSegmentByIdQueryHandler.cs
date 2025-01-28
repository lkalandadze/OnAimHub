using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById;

public sealed class GetSegmentByIdQueryHandler : IQueryHandler<GetSegmentByIdQuery, ApplicationResult<SegmentDto>>
{
    private readonly ISegmentService _segmentService;

    public GetSegmentByIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult<SegmentDto>> Handle(GetSegmentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _segmentService.GetById(request.SegmentId);
    }
}
