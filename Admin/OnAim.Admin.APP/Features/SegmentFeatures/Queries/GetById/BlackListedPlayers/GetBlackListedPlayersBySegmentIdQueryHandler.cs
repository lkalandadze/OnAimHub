using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;

public class GetBlackListedPlayersBySegmentIdQueryHandler : IQueryHandler<GetBlackListedPlayersBySegmentIdQuery, ApplicationResult<PaginatedResult<SegmentPlayerDto>>>
{
    private readonly ISegmentService _segmentService;

    public GetBlackListedPlayersBySegmentIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult<PaginatedResult<SegmentPlayerDto>>> Handle(GetBlackListedPlayersBySegmentIdQuery request, CancellationToken cancellationToken)
    {
        return await _segmentService.GetBlackListedPlayers(request.SegmentId, request.Filter);
    }
}
