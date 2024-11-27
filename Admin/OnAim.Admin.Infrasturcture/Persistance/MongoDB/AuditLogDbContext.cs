using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Persistance.MongoDB;

public class AuditLogDbContext : MongoDbContext
{
    public AuditLogDbContext(IOptions<MongoDbOptions> options) : base(options)
    {
        AuditLogs = GetCollection<AuditLog>();
        AccessDeniedLogs = GetCollection<AccessDeniedLog>();
        OperationFailedLogs = GetCollection<OperationFailedLog>();
        CoinTemplates = GetCollection<CoinTemplate>();
        PromotionViews = GetCollection<PromotionViewTemplate>();
        WithdrawEndpointTemplates = GetCollection<WithdrawEndpointTemplate>();
    }

    public IMongoCollection<AuditLog> AuditLogs { get; }
    public IMongoCollection<AccessDeniedLog> AccessDeniedLogs { get; }
    public IMongoCollection<OperationFailedLog> OperationFailedLogs { get; }
    public IMongoCollection<CoinTemplate> CoinTemplates { get; }
    public IMongoCollection<PromotionViewTemplate> PromotionViews { get; }
    public IMongoCollection<WithdrawEndpointTemplate> WithdrawEndpointTemplates { get; }
}
