﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.AuditLog;
using OnAim.Admin.Shared.Paging;

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
        {
            logs = logs.Where(x => request.Filter.Actions.Contains(x.Action)).ToList();
        }

        if (request.Filter.Categories != null && request.Filter.Categories.Any())
        {
            logs = logs.Where(x => request.Filter.Categories.Contains(x.Category)).ToList();
        }

        if (request.Filter.DateFrom.HasValue)
        {
            logs = logs.Where(x => x.Timestamp >= request.Filter.DateFrom.Value).ToList();
        }

        if (request.Filter.DateTo.HasValue)
        {
            logs = logs.Where(x => x.Timestamp <= request.Filter.DateTo.Value).ToList();
        }

        var totalCount = logs.Count;

        var pageNumber = request.Filter.PageNumber ?? 1;
        var pageSize = request.Filter.PageSize ?? 25;

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
}
