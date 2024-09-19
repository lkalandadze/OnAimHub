using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.AuditLog;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Queries.User.GetUserLogs
{
    public class GetUserLogsQueryHandler : IQueryHandler<GetUserLogsQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;
        private readonly IConfigurationRepository<AuditLog> _auditLogRepository;

        public GetUserLogsQueryHandler(
            IRepository<Infrasturcture.Entities.User> repository,
            IConfigurationRepository<AuditLog> auditLogRepository
            )
        {
            _repository = repository;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<ApplicationResult> Handle(GetUserLogsQuery request, CancellationToken cancellationToken)
        {
            var logs = _auditLogRepository.Query(x => x.UserId == request.Id);

            if (request.Filter.Actions != null && request.Filter.Actions.Any())
            {
                logs = logs.Where(x => x.Action == x.Action);
            }

            //if (request.Filter.Types != null && request.Filter.Types.Any())
            //{
            //    logs = logs.Where(x => x.Type == x.Type);
            //}

            if (request.Filter.DateFrom.HasValue)
            {
                logs = logs.Where(x => x.Timestamp >= request.Filter.DateFrom.Value);
            }
            if (request.Filter.DateTo.HasValue)
            {
                logs = logs.Where(x => x.Timestamp <= request.Filter.DateTo.Value);
            }

            var totalCount = await logs.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;


            var res = logs.Select(x => new AuditLogDto
            {
                Id = x.Id,
                Action = x.Action,
                Log = x.Log,
                Timestamp = x.Timestamp,
            })
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize);

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<AuditLogDto>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = await res.ToListAsync()
                }
            };
        }
    }
}
