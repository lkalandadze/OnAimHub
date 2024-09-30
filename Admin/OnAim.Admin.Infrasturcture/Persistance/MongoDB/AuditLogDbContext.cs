using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities;

namespace OnAim.Admin.Infrasturcture.Persistance.MongoDB;

public class AuditLogDbContext : MongoDbContext
{
    public AuditLogDbContext(IOptions<MongoDbOptions> options) : base(options)
    {
        AuditLogs = GetCollection<AuditLog>();
        RejectedLogs = GetCollection<RejectedLog>();
    }

    public IMongoCollection<AuditLog> AuditLogs { get; }
    public IMongoCollection<RejectedLog> RejectedLogs { get; }
}
