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

    public async Task AddRejectedLogAsync(RejectedLog rejectedLog)
    {
        await _context.RejectedLogs.InsertOneAsync(rejectedLog);
    }

    public async Task<List<AuditLog>> GetUserLogs(int userId)
    {
        var filter = Builders<AuditLog>.Filter.Eq(log => log.UserId, userId);
        return await _context.AuditLogs.Find(filter).ToListAsync();
    }

}
