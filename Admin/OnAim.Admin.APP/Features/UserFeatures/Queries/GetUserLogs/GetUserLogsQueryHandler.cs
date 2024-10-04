using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.AuditLog;
using OnAim.Admin.Shared.Paging;
using System.Linq.Dynamic.Core;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;

public class GetUserLogsQueryHandler : IQueryHandler<GetUserLogsQuery, ApplicationResult>
{
    private readonly IRepository<User> _repository;
    private readonly ILogRepository _logRepository;

    public GetUserLogsQueryHandler(
        IRepository<User> repository,
        ILogRepository logRepository
        )
    {
        _repository = repository;
        _logRepository = logRepository;
    }

    public async Task<ApplicationResult> Handle(GetUserLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _logRepository.GetUserLogs(request.Id);

        if (request.Filter.Actions != null && request.Filter.Actions.Any())
            logs = logs.Where(x => request.Filter.Actions.Contains(x.Action)).ToList();

        if (request.Filter.Categories != null && request.Filter.Categories.Any())
            logs = logs.Where(x => request.Filter.Categories.Contains(x.Category)).ToList();

        if (request.Filter.DateFrom.HasValue)
            logs = logs.Where(x => x.Timestamp >= request.Filter.DateFrom.Value).ToList();

        if (request.Filter.DateTo.HasValue)
            logs = logs.Where(x => x.Timestamp <= request.Filter.DateTo.Value).ToList();

        var totalCount = logs.Count;

        var pageNumber = request.Filter.PageNumber ?? 1;
        var pageSize = request.Filter.PageSize ?? 25;

        var sortBy = request.Filter.SortBy ?? "id";
        var sortDescending = request.Filter.SortDescending ?? false;

        logs = SortLogs(logs, sortBy, sortDescending).ToList();

        var pagedLogs = logs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(log => new AuditLogDto
            {
                Id = log.Id,
                Action = log.Action,
                Log = log.Log,
                Timestamp = log.Timestamp
            })
            .ToList();

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<AuditLogDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = pagedLogs
            }
        };
    }
    private IEnumerable<AuditLog> SortLogs(IEnumerable<AuditLog> logs, string sortBy, bool descending)
    {
        var sortDirection = descending ? " descending" : string.Empty;
        var orderBy = $"{sortBy}{sortDirection}";

        return logs.AsQueryable().OrderBy(orderBy);
    }
    public List<dynamic> Sort(List<dynamic> input, string property)
    {
        return input.OrderBy(p => p.GetType()
                                   .GetProperty(property)
                                   .GetValue(p, null)).ToList();
    }
}
