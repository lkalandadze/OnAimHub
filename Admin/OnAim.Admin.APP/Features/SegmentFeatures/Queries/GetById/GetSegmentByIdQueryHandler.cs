using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById
{
    public sealed class GetSegmentByIdQueryHandler : IQueryHandler<GetSegmentByIdQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<Segment> _readOnlyRepository;
        private readonly IReadOnlyRepository<PlayerSegmentAct> _playerSegmentActRepository;

        public GetSegmentByIdQueryHandler(IReadOnlyRepository<Segment> readOnlyRepository, IReadOnlyRepository<PlayerSegmentAct> playerSegmentActRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _playerSegmentActRepository = playerSegmentActRepository;
        }

        public async Task<ApplicationResult> Handle(GetSegmentByIdQuery request, CancellationToken cancellationToken)
        {
            var segment = await _readOnlyRepository
                .Query(x => x.Id == request.SegmentId)
                .Include(x => x.PlayerSegments)
                .Include(x => x.PlayerBlockedSegments)
                .FirstOrDefaultAsync();

            var playerSegmentActs = await _playerSegmentActRepository.Query(x => x.SegmentId == request.SegmentId).ToListAsync();

            var res = new SegmentDto
            {
                SegmentId = segment.Id,
                SegmentName = segment.Description,
                PriorityLevel = segment.PriorityLevel,
                TotalActivePlayers = playerSegmentActs.Select(x => x.TotalPlayers).Count(),
                TotalBlackListedPlayers = playerSegmentActs.Select(x => x.TotalPlayers).Count(),
                ActivePlayers = segment.PlayerSegments.Select(x => new Admin.Shared.DTOs.Player.PlayerDto
                {
                    Id = x.Id,
                    PlayerName = x.Player.UserName
                }).ToList(),
                BlackListedPlayers = segment.PlayerBlockedSegments.Select(x => new Admin.Shared.DTOs.Player.PlayerDto
                {
                    Id = x.PlayerId,
                    PlayerName = x.Player.UserName
                }).ToList(),
                Acts = playerSegmentActs.Select(x => new ActsDto
                {
                    Id=x.Id,
                    UploadedBy = x.ByUserId,
                    Quantity = x.TotalPlayers,
                    Type = x.Action.ToString(),
                }).ToList(),
               History = null, //TODO
            };

            return new ApplicationResult { Success = true, Data = res };
        }
    }
}
