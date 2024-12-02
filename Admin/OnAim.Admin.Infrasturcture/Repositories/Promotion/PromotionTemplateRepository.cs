using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.Infrasturcture.Repositories.Promotion;

public class PromotionTemplateRepository : IPromotionTemplateRepository
{
    private readonly AuditLogDbContext _dbContext;

    public PromotionTemplateRepository(AuditLogDbContext context)
    {
        _dbContext = context;
    }

    public async Task AddPromotionTemplateAsync(PromotionTemplate template)
    {
        await _dbContext.PromotionTemplates.InsertOneAsync(template);
    }

    public async Task<List<PromotionTemplate>> GetPromotionTemplates()
    {
        return await _dbContext.PromotionTemplates.Find(_ => true).ToListAsync();
    }

    public async Task<PromotionTemplate?> GetPromotionTemplateByIdAsync(string id)
    {
        var filter = Builders<PromotionTemplate>.Filter.Eq(ct => ct.Id, id);
        return await _dbContext.PromotionTemplates.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<PromotionTemplate?> UpdatePromotionTemplateAsync(string id, PromotionTemplate updatedCoinTemplate)
    {
        var filter = Builders<PromotionTemplate>.Filter.Eq(ct => ct.Id, id);
        var result = await _dbContext.PromotionTemplates.ReplaceOneAsync(filter, updatedCoinTemplate);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return updatedCoinTemplate;
        }

        return null;
    }
}
