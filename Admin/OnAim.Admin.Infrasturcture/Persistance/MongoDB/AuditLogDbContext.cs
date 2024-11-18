using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Infrasturcture.Persistance.MongoDB;

public class AuditLogDbContext : MongoDbContext
{
    public AuditLogDbContext(IOptions<MongoDbOptions> options) : base(options)
    {
        AuditLogs = GetCollection<AuditLog>();
        AccessDeniedLogs = GetCollection<AccessDeniedLog>();
        OperationFailedLogs = GetCollection<OperationFailedLog>();
        Promotions = GetCollection<Promotion>();
    }

    public IMongoCollection<AuditLog> AuditLogs { get; }
    public IMongoCollection<AccessDeniedLog> AccessDeniedLogs { get; }
    public IMongoCollection<OperationFailedLog> OperationFailedLogs { get; }
    public IMongoCollection<Promotion> Promotions { get; }
}
