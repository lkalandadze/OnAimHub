using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.History;

public class GetSegmentActsHistoryByIdQueryHandler : IQueryHandler<GetSegmentActsHistoryByIdQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<PlayerSegmentActHistory> _playerSegmentActHistoryRepository;

    public GetSegmentActsHistoryByIdQueryHandler(IReadOnlyRepository<PlayerSegmentActHistory> playerSegmentActHistoryRepository)
    {
        _playerSegmentActHistoryRepository = playerSegmentActHistoryRepository;
    }
    public async Task<ApplicationResult> Handle(GetSegmentActsHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var history = await _playerSegmentActHistoryRepository.Query(x => x.PlayerSegmentActId == request.PlayerSegmentActId).ToListAsync();

        var res = history.Select(x => new ActsHistoryDto
        {
            Id = x.Id,
            Note = null,
            PlayerName = x.Player.UserName,
            PlayerId = x.PlayerId,
            Quantity = 1,
            UploadedBy = x.PlayerSegmentAct.ByUserId,
            UploadedOn = null,
            Type = x.PlayerSegmentAct.Action.Name,
        });

        return new ApplicationResult
        {
            Success = true,
            Data = res
        };
    }
}
