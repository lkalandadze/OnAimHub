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
        var sortableFields = new List<string> { "Id", "UserName" };

        var palyers = _readOnlyRepository.Query(x =>
                        string.IsNullOrEmpty(request.Filter.Name) || x.UserName.ToLower().Contains(request.Filter.Name.ToLower()));

        if (request.Filter.IsBanned.HasValue)
            palyers = palyers.Where(x => x.IsBanned == request.Filter.IsBanned.Value);

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

        if (request.Filter.SortBy == "Id" || request.Filter.SortBy == "id")
        {
            palyers = sortDescending
                ? palyers.OrderByDescending(x => x.Id)
                : palyers.OrderBy(x => x.Id);
        }
        else if (request.Filter.SortBy == "userName" || request.Filter.SortBy == "UserName")
        {
            palyers = sortDescending
                ? palyers.OrderByDescending(x => x.UserName)
                : palyers.OrderBy(x => x.UserName);
        }

        var res = palyers
            .Select(x => new PlayerListDto
            {
                Id = x.Id,
                UserName = x.UserName ?? null,
                RegistrationDate = null,
                LastVisit = null,
                Segment = x.PlayerSegments
                            .OrderByDescending(ps => ps.Segment.PriorityLevel)
                            .Select(ps => ps.Segment.Id)
                            .FirstOrDefault(),
                IsBanned = x.IsBanned,
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
                Items = await res.ToListAsync(),
                SortableFields = sortableFields,
            },
        };
    }
}
