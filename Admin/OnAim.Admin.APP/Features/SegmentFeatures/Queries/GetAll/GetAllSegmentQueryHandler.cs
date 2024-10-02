using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;

public sealed class GetAllSegmentQueryHandler : IQueryHandler<GetAllSegmentQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<Segment> _readOnlyRepository;

    public GetAllSegmentQueryHandler(IReadOnlyRepository<Segment> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<ApplicationResult> Handle(GetAllSegmentQuery request, CancellationToken cancellationToken)
    {
        var segments = _readOnlyRepository.Query().Include(x => x.PlayerSegments);

        var totalCount = await segments.CountAsync(cancellationToken);

        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? 25;

        var res = segments
       .Select(x => new SegmentListDto
       {
           Id = x.Id,
           Name = x.Id,
           Description = x.Description,
           Priority = x.PriorityLevel,
           TotalPlayers = x.PlayerSegments.Count(),
           LastUpdate = null,
       })
       .Skip((pageNumber - 1) * pageSize)
       .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<SegmentListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync()
            },
        };
    }
}
