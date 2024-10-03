using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetAll;

public class GetAllPlayerQueryHandler : IQueryHandler<GetAllPlayerQuery, ApplicationResult>
{
    private readonly IReadOnlyRepository<Domain.HubEntities.Player> _readOnlyRepository;

    public GetAllPlayerQueryHandler(IReadOnlyRepository<Domain.HubEntities.Player> readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;
    }
    public async Task<ApplicationResult> Handle(GetAllPlayerQuery request, CancellationToken cancellationToken)
    {
        var palyers = _readOnlyRepository.Query(x =>
                        string.IsNullOrEmpty(request.Filter.Name) || x.UserName.Contains(request.Filter.Name));

        //if(request.Filter.Status != null)
        //    palyers.Select(x => x.Status == request.Filter.Status);

        if (request.Filter.SegmentIds?.Any() == true)
            palyers = palyers.Where(x => x.PlayerSegments.Any(ur => request.Filter.SegmentIds.Contains(ur.SegmentId)));

        if (request.Filter.DateFrom.HasValue)
            palyers = palyers.Where(x => x.LastVisitedOn >= request.Filter.DateFrom.Value);

        if (request.Filter.DateFrom.HasValue)
            palyers = palyers.Where(x => x.LastVisitedOn <= request.Filter.DateTo.Value);

        var totalCount = await palyers.CountAsync(cancellationToken);

        var pageNumber = request.Filter.PageNumber ?? 1;
        var pageSize = request.Filter.PageSize ?? 25;

        bool sortDescending = request.Filter.SortDescending.GetValueOrDefault();

        if (request.Filter.SortBy == "playerId" || request.Filter.SortBy == "PlayerId")
        {
            palyers = sortDescending
                ? palyers.OrderByDescending(x => x.Id)
                : palyers.OrderBy(x => x.Id);
        }
        else if (request.Filter.SortBy == "playerName" || request.Filter.SortBy == "PlayerName")
        {
            palyers = sortDescending
                ? palyers.OrderByDescending(x => x.UserName)
                : palyers.OrderBy(x => x.UserName);
        }

        var res = palyers
            .Select(x => new PlayerListDto
            {
                PlayerId = x.Id,
                PlayerName = x.UserName ?? null,
                RegistrationDate = null,
                LastVisit = null,
                Segment = x.PlayerSegments.Select(x => x.Segment.Id).FirstOrDefault(),
                Status = null,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);


        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<PlayerListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync()
            },
        };
    }
}
