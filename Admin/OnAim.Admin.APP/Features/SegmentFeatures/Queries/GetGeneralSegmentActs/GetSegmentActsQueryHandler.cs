using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetSegmentActs;

public class GetSegmentActsQueryHandler : IQueryHandler<GetSegmentActsQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<PlayerSegmentAct> _readOnlyRepository;

    public GetSegmentActsQueryHandler(IReadOnlyRepository<PlayerSegmentAct> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<ApplicationResult> Handle(GetSegmentActsQuery request, CancellationToken cancellationToken)
    {
        var query = _readOnlyRepository.Query();

        //if (request.Filter.DateFrom.HasValue)
        //    query = query.Where(x => x.DateCreated >= request.Filter.DateFrom.Value);

        //if (request.Filter.DateTo.HasValue)
        //    query = query.Where(x => x.DateCreated <= request.Filter.DateTo.Value);

        if (request.Filter.SegmentId != null)
            query = query.Where(x => x.Segment.Id == request.Filter.SegmentId);

        if (request.Filter.UserId != null)
        {
            query = query.Where(x => x.ByUserId == request.Filter.UserId);
        }

        var aa = await query.ToListAsync(cancellationToken);

        return new ApplicationResult
        {
            Success = true,
            Data = aa,
        };
    }
}
