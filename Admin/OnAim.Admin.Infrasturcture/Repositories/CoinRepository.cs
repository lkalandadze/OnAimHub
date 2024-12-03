using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.Infrasturcture.Repositories;

public class CoinRepository : ICoinRepository
{
    private readonly AuditLogDbContext _dbContext;

    public CoinRepository(AuditLogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddCoinTemplateAsync(CoinTemplate coinTemplate)
    {
        await _dbContext.CoinTemplates.InsertOneAsync(coinTemplate);
    }

    public async Task<List<CoinTemplate>> GetCoinTemplates()
    {
        return await _dbContext.CoinTemplates
            .Find(template => !template.IsDeleted)
            .ToListAsync();
    }

    public async Task<CoinTemplate?> GetCoinTemplateByIdAsync(string id)
    {
        var filter = Builders<CoinTemplate>.Filter.Eq(ct => ct.Id, id);
        return await _dbContext.CoinTemplates.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<CoinTemplate?> UpdateCoinTemplateAsync(string id, CoinTemplate updatedCoinTemplate)
    {
        var filter = Builders<CoinTemplate>.Filter.Eq(ct => ct.Id, id);
        var result = await _dbContext.CoinTemplates.ReplaceOneAsync(filter, updatedCoinTemplate);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return updatedCoinTemplate; 
        }

        return null;
    }

}
