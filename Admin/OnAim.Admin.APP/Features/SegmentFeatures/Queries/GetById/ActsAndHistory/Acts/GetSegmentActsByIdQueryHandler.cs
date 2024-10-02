using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.Acts
{
    public class GetSegmentActsByIdQueryHandler : IQueryHandler<GetSegmentActsByIdQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<PlayerSegmentAct> _playerSegmentActRepository;

        public GetSegmentActsByIdQueryHandler(IReadOnlyRepository<PlayerSegmentAct> playerSegmentActRepository)
        {
            _playerSegmentActRepository = playerSegmentActRepository;
        }
        public async Task<ApplicationResult> Handle(GetSegmentActsByIdQuery request, CancellationToken cancellationToken)
        {
            var playerSegmentActs = await _playerSegmentActRepository.Query(x => x.SegmentId == request.SegmentId).Include(x => x.Action).ToListAsync();

            var res = playerSegmentActs.Select(x => new ActsDto
            {
                Id = x.Id,
                UploadedBy = x.ByUserId,
                Quantity = x.TotalPlayers,
                Type = x.Action?.Name,
            });

            return new ApplicationResult
            {
                Success = true,
                Data = res
            };
        }
    }
}
