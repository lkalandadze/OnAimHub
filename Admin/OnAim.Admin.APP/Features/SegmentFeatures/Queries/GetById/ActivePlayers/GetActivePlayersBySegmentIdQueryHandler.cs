using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActivePlayers;

public class GetActivePlayersBySegmentIdQueryHandler : IQueryHandler<GetActivePlayersBySegmentIdQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<PlayerSegment> _playerSegmentRepository;
    private readonly IReadOnlyRepository<PlayerBlockedSegment> _playerBlockedSegmentRepository;

    public GetActivePlayersBySegmentIdQueryHandler(IReadOnlyRepository<PlayerSegment> playerSegmentRepository, IReadOnlyRepository<PlayerBlockedSegment> playerBlockedSegmentRepository)
    {
        _playerSegmentRepository = playerSegmentRepository;
        _playerBlockedSegmentRepository = playerBlockedSegmentRepository;
    }

    public async Task<ApplicationResult> Handle(GetActivePlayersBySegmentIdQuery request, CancellationToken cancellationToken)
    {
        if (request.SegmentId == null)
            throw new BadRequestException("Segment Not Found");

        var blacklistedPlayerIds = await _playerBlockedSegmentRepository
            .Query(x => x.SegmentId == request.SegmentId)
            .Select(x => x.PlayerId)
            .ToListAsync(cancellationToken);

        var activePlayers = _playerSegmentRepository
            .Query(x => x.SegmentId == request.SegmentId && !blacklistedPlayerIds.Contains(x.PlayerId));

        if(request.Filter.UserName != null)
            activePlayers = activePlayers.Where(x => x.Player.UserName == request.Filter.UserName);

        var totalCount = activePlayers.Count();

        var pageNumber = request.Filter.PageNumber ?? 1;
        var pageSize = request.Filter.PageSize ?? 25;

        var paged = await activePlayers
            .Select(x => new SegmentPlayerDto
            {
                PlayerId = x.PlayerId,
                PlayerName = x.Player.UserName
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<SegmentPlayerDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = paged
            },
        };
    }
}
