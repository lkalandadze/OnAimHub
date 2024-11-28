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
        LeaderboardTemplates = GetCollection<LeaderboardTemplate>();
        LeaderboardTemplatePrizes = GetCollection<LeaderboardTemplatePrize>();
    }

    public IMongoCollection<AuditLog> AuditLogs { get; }
    public IMongoCollection<AccessDeniedLog> AccessDeniedLogs { get; }
    public IMongoCollection<OperationFailedLog> OperationFailedLogs { get; }
    public IMongoCollection<CoinTemplate> CoinTemplates { get; }
    public IMongoCollection<PromotionViewTemplate> PromotionViews { get; }
    public IMongoCollection<WithdrawEndpointTemplate> WithdrawEndpointTemplates { get; }
    public IMongoCollection<LeaderboardTemplate> LeaderboardTemplates { get; }
    public IMongoCollection<LeaderboardTemplatePrize> LeaderboardTemplatePrizes { get; }
}
