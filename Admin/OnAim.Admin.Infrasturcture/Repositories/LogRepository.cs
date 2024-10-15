using MongoDB.Driver;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Repositories;

public class LogRepository : ILogRepository
{
    private AuditLogDbContext _context;

    public LogRepository(AuditLogDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task AddAuditLogAsync(AuditLog auditLog)
    {
        auditLog.Timestamp = SystemDate.Now;
        await _context.AuditLogs.InsertOneAsync(auditLog);
    }

    public async Task AddOperationFailedLogAsync(OperationFailedLog operationFailedLog)
    {
        await _context.OperationFailedLogs.InsertOneAsync(operationFailedLog);
    }

    public async Task AddAccessDeniedLogAsync(AccessDeniedLog accessDeniedLog)
    {
        await _context.AccessDeniedLogs.InsertOneAsync(accessDeniedLog);
    }

    public async Task<List<AuditLog>> GetUserLogs(int userId)
    {
        var filter = Builders<AuditLog>.Filter.Eq(log => log.UserId, userId);
        return await _context.AuditLogs.Find(filter).ToListAsync();
    }

}
