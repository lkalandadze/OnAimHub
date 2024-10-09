using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetGeneralSegmentActsHistory;

public class GetGeneralSegmentActsHistoryQueryHandler : IQueryHandler<GetGeneralSegmentActsHistoryQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<PlayerSegmentActHistory> _readOnlyRepository;

    public GetGeneralSegmentActsHistoryQueryHandler(IReadOnlyRepository<PlayerSegmentActHistory> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<ApplicationResult> Handle(GetGeneralSegmentActsHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _readOnlyRepository.Query();

        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = request.Filter.PageNumber ?? 1;
        var pageSize = request.Filter.PageSize ?? 25;

        if (request.Filter.SegmentId != null)
        {
            query = query.Where(x => x.Player.PlayerSegments.Any(ur => request.Filter.SegmentId.Contains(ur.SegmentId)));
        }

        if (request.Filter.UserId.HasValue && request.Filter.UserId.Value != 0)
        {
            query = query.Where(x => x.PlayerId == request.Filter.UserId.Value);
        }

        var res = await query
            .Select(x => new ActsHistoryDto
            {
                Id = x.Id,
                Note = null,
                PlayerId = x.PlayerId,
                PlayerName = x.Player.UserName ?? "Unknown",
                UploadedBy = null,
                UploadedOn = null,
                Quantity = x.PlayerSegmentAct.TotalPlayers,
                Type = x.PlayerSegmentAct.Action.Name ?? null
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<ActsHistoryDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res,
            },
        };
    }
}
