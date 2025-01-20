using MongoDB.Driver;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.Infrasturcture.Repositories;

public class GameConfigurationTemplateRepository : IGameConfigurationTemplateRepository
{
    private readonly AuditLogDbContext _dbContext;

    public GameConfigurationTemplateRepository(AuditLogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddGameConfigurationTemplateAsync(GameConfigurationTemplate template)
    {
        await _dbContext.GameConfigurationTemplates.InsertOneAsync(template);
    }

    public async Task<List<GameConfigurationTemplate>> GetGameConfigurationTemplates()
    {
        var templates = await _dbContext.GameConfigurationTemplates.Find(_ => true).ToListAsync();

        return templates.Cast<GameConfigurationTemplate>().ToList();
    }

    public async Task<GameConfigurationTemplate?> GetGameConfigurationTemplateByIdAsync(string id)
    {
        var filter = Builders<GameConfigurationTemplate>.Filter.Eq("Id", id);
        var result = await _dbContext.GameConfigurationTemplates.Find(filter).FirstOrDefaultAsync();

        return result;
    }

    public async Task<GameConfigurationTemplate?> UpdateGameConfigurationTemplateAsync(string id, GameConfigurationTemplate updated)
    {
        var filter = Builders<GameConfigurationTemplate>.Filter.Eq("Id", id);
        var result = await _dbContext.GameConfigurationTemplates.ReplaceOneAsync(filter, updated);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return updated;
        }

        return null;
    }
}
