using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Queries.Player.GetAll
{
    public class GetAllPlayerQueryHandler : IQueryHandler<GetAllPlayerQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<Infrasturcture.HubEntities.Player> _readOnlyRepository;

        public GetAllPlayerQueryHandler(IReadOnlyRepository<Infrasturcture.HubEntities.Player> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }
        public async Task<ApplicationResult> Handle(GetAllPlayerQuery request, CancellationToken cancellationToken)
        {
            var palyers = _readOnlyRepository.Query();

            var totalCount = await palyers.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;

            var res = palyers
                .Select(x => new PlayerListDto
                {
                    Id = x.Id,
                    PlayerName = x.UserName,
                    RegistrationDate = null,
                    LastVisit = null,
                    Segment = x.PlayerSegments.Select(x => x.Segment.Description).FirstOrDefault(),
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
}
