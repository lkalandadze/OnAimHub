using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActivePlayers;

public class GetActivePlayersBySegmentIdQueryHandler : IQueryHandler<GetActivePlayersBySegmentIdQuery, ApplicationResult<PaginatedResult<SegmentPlayerDto>>>
{
    private readonly ISegmentService _segmentService;

    public GetActivePlayersBySegmentIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult<PaginatedResult<SegmentPlayerDto>>> Handle(GetActivePlayersBySegmentIdQuery request, CancellationToken cancellationToken)
    {      
        return await _segmentService.GetActivePlayers(request.SegmentId, request.Filter);
    }
}
