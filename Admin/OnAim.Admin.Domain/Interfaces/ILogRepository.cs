﻿using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Domain.Interfaces;

public interface ILogRepository
{
    Task AddAuditLogAsync(AuditLog auditLog);
    Task AddOperationFailedLogAsync(OperationFailedLog operationFailedLog);
    Task AddAccessDeniedLogAsync(AccessDeniedLog accessDeniedLog);
    Task<List<AuditLog>> GetUserLogs(int userId);
}
