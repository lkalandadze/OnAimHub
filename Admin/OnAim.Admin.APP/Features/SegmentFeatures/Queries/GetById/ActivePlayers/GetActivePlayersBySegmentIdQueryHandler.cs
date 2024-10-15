using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActivePlayers;

public class GetActivePlayersBySegmentIdQueryHandler : IQueryHandler<GetActivePlayersBySegmentIdQuery, ApplicationResult>
{
    private readonly ISegmentService _segmentService;

    public GetActivePlayersBySegmentIdQueryHandler(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    public async Task<ApplicationResult> Handle(GetActivePlayersBySegmentIdQuery request, CancellationToken cancellationToken)
    {      
        var result = await _segmentService.GetActivePlayers(request.SegmentId, request.Filter);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
