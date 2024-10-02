using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById
{
    public sealed class GetSegmentByIdQueryHandler : IQueryHandler<GetSegmentByIdQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<Segment> _segmentRepository;
        private readonly IReadOnlyRepository<PlayerSegment> _playerSegmentRepository;
        private readonly IReadOnlyRepository<PlayerBlockedSegment> _playerBlockedSegmentRepository;

        public GetSegmentByIdQueryHandler(
            IReadOnlyRepository<Segment> segmentRepository,
            IReadOnlyRepository<PlayerSegment> playerSegmentRepository,
            IReadOnlyRepository<PlayerBlockedSegment> playerBlockedSegmentRepository)
        {
            _segmentRepository = segmentRepository;
            _playerSegmentRepository = playerSegmentRepository;
            _playerBlockedSegmentRepository = playerBlockedSegmentRepository;
        }

        public async Task<ApplicationResult> Handle(GetSegmentByIdQuery request, CancellationToken cancellationToken)
        {
            var segment = await _segmentRepository
                .Query(x => x.Id == request.SegmentId)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.PriorityLevel
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (segment == null)
            {
                throw new NotFoundException("Segment not found.");
            }

            var blacklistedPlayerIds = await _playerBlockedSegmentRepository
                .Query(x => x.SegmentId == segment.Id)
                .Select(x => x.PlayerId)
                .ToListAsync(cancellationToken);

            var activePlayers = await _playerSegmentRepository
                .Query(x => x.SegmentId == segment.Id && !blacklistedPlayerIds.Contains(x.PlayerId))
                .Select(x => new SegmentPlayerDto
                {
                    PlayerId = x.PlayerId,
                    PlayerName = x.Player.UserName
                })
                .ToListAsync(cancellationToken);

            var blacklistedPlayers = await _playerBlockedSegmentRepository
                .Query(x => x.SegmentId == segment.Id)
                .Select(bp => new SegmentPlayerDto
                {
                    PlayerId = bp.PlayerId,
                    PlayerName = bp.Player.UserName
                })
                .ToListAsync(cancellationToken);

            var res = new SegmentDto
            {
                SegmentId = segment.Id,
                SegmentName = segment.Id,
                PriorityLevel = segment.PriorityLevel,
                TotalActivePlayers = activePlayers.Count,
                TotalBlackListedPlayers = blacklistedPlayers.Count,
                ActivePlayers = activePlayers,
                BlackListedPlayers = blacklistedPlayers
            };

            return new ApplicationResult { Success = true, Data = res };
        }
    }
}
