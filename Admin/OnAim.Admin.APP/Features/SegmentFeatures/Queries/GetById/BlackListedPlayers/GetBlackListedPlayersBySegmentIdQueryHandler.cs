using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;

public class GetBlackListedPlayersBySegmentIdQueryHandler : IQueryHandler<GetBlackListedPlayersBySegmentIdQuery, ApplicationResult>
{
    private readonly ISegmentService _segmentService;

    public GetBlackListedPlayersBySegmentIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult> Handle(GetBlackListedPlayersBySegmentIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _segmentService.GetBlackListedPlayers(request.SegmentId, request.Filter);

        return new ApplicationResult
        { 
            Success = result.Success,
            Data = result.Data
        };

    }
}
