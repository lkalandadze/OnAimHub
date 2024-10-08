using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetSegmentActs
{
    public class GetSegmentActsQueryHandler : IQueryHandler<GetSegmentActsQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<PlayerSegmentAct> _readOnlyRepository;

        public GetSegmentActsQueryHandler(IReadOnlyRepository<PlayerSegmentAct> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<ApplicationResult> Handle(GetSegmentActsQuery request, CancellationToken cancellationToken)
        {
            var query = _readOnlyRepository.Query();

            if (request.Filter.SegmentId != null)
                query = query.Where(x => x.SegmentId == request.Filter.SegmentId);

            if (request.Filter.UserId != null)
                query = query.Where(x => x.ByUserId == request.Filter.UserId);

            //if (request.Filter.DateFrom.HasValue)
            //    query = query.Where(x => x.DateCreated >= request.Filter.DateFrom.Value);

            //if (request.Filter.DateTo.HasValue)
            //    query = query.Where(x => x.DateCreated <= request.Filter.DateTo.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;

            bool sortDescending = request.Filter.SortDescending.GetValueOrDefault();

            if (request.Filter.SortBy == "id" || request.Filter.SortBy == "Id")
            {
                query = sortDescending
                    ? query.OrderByDescending(x => x.Id)
                    : query.OrderBy(x => x.Id);
            }
            //else if (request.Filter.SortBy == "playerName" || request.Filter.SortBy == "PlayerName")
            //{
            //    query = sortDescending
            //        ? query.OrderByDescending(x => x.UserName)
            //        : query.OrderBy(x => x.UserName);
            //}

            var res = query.Select(x => new ActsDto
            {
                Id = x.Id,
                Note = null,
                UploadedBy = x.ByUserId,
                Quantity = x.TotalPlayers,
                Type = x.Action.Name,
                UploadedOn = null
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

            //var results = await query.Include(x => x.Action).ToListAsync(cancellationToken);

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<ActsDto>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = await res.ToListAsync()
                },
            };
        }
    }
}
