using MongoDB.Bson;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.Infrasturcture.Repositories.Leaderboard;

public class LeaderboardTemplateRepository : ILeaderboardTemplateRepository
{
    private readonly AuditLogDbContext _dbContext;

    public LeaderboardTemplateRepository(AuditLogDbContext auditLogDbContext)
    {
        _dbContext = auditLogDbContext;
    }

    public async Task AddLeaderboardTemplateAsync(LeaderboardTemplate template)
    {
        await _dbContext.LeaderboardTemplates.InsertOneAsync(template);
    }

    public async Task<List<LeaderboardTemplate>> GetLeaderboardTemplates()
    {
        return await _dbContext.LeaderboardTemplates.Find(_ => true).ToListAsync();
    }

    public async Task<LeaderboardTemplate?> GetLeaderboardTemplateByIdAsync(ObjectId id)
    {
        var filter = Builders<LeaderboardTemplate>.Filter.Eq(ct => ct.Id, id);
        return await _dbContext.LeaderboardTemplates.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<LeaderboardTemplate?> UpdateLeaderboardTemplateAsync(ObjectId id, LeaderboardTemplate updated)
    {
        var filter = Builders<LeaderboardTemplate>.Filter.Eq(ct => ct.Id, id);
        var result = await _dbContext.LeaderboardTemplates.ReplaceOneAsync(filter, updated);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return updated;
        }

        return null;
    }
}
