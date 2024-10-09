using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;

public class GetBlackListedPlayersBySegmentIdQueryHandler : IQueryHandler<GetBlackListedPlayersBySegmentIdQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<Segment> _segmentRepository;
    private readonly IReadOnlyRepository<PlayerSegment> _playerSegmentRepository;
    private readonly IReadOnlyRepository<PlayerBlockedSegment> _playerBlockedSegmentRepository;

    public GetBlackListedPlayersBySegmentIdQueryHandler(
        IReadOnlyRepository<Segment> segmentRepository,
        IReadOnlyRepository<PlayerSegment> playerSegmentRepository,
        IReadOnlyRepository<PlayerBlockedSegment> playerBlockedSegmentRepository
        )
    {
        _segmentRepository = segmentRepository;
        _playerSegmentRepository = playerSegmentRepository;
        _playerBlockedSegmentRepository = playerBlockedSegmentRepository;
    }

    public async Task<ApplicationResult> Handle(GetBlackListedPlayersBySegmentIdQuery request, CancellationToken cancellationToken)
    {
        if (request.SegmentId == null)
            throw new BadRequestException("Segment Not Found");     

        var blacklistedPlayerIds = await _playerBlockedSegmentRepository
            .Query(x => x.SegmentId == request.SegmentId)
            .Select(x => x.PlayerId)
            .ToListAsync(cancellationToken);

        var activePlayers = await _playerSegmentRepository
            .Query(x => x.SegmentId == request.SegmentId && !blacklistedPlayerIds.Contains(x.PlayerId))
            .Select(x => new SegmentPlayerDto
            {
                PlayerId = x.PlayerId,
                PlayerName = x.Player.UserName
            })
            .ToListAsync(cancellationToken);

        var blacklistedPlayers = _playerBlockedSegmentRepository
            .Query(x => x.SegmentId == request.SegmentId);

        if (request.Filter.UserName != null)
            blacklistedPlayers = blacklistedPlayers.Where(x => x.Player.UserName == request.Filter.UserName);

        var totalCount = blacklistedPlayers.Count();

        var pageNumber = request.Filter.PageNumber ?? 1;
        var pageSize = request.Filter.PageSize ?? 25;

        var pagedList = await blacklistedPlayers
         .Select(bp => new SegmentPlayerDto
         {
             PlayerId = bp.Player.Id,
             PlayerName = bp.Player.UserName,
             ReasonNote = null
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
                Items = pagedList
            },
        };

    }
}
