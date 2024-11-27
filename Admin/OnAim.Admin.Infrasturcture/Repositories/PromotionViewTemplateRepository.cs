using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.Infrasturcture.Repositories;

public class PromotionViewTemplateRepository : IPromotionViewTemplateRepository
{
    private readonly AuditLogDbContext _dbContext;

    public PromotionViewTemplateRepository(AuditLogDbContext auditLogDbContext)
    {
        _dbContext = auditLogDbContext;
    }

    public async Task AddPromotionViewTemplateAsync(PromotionViewTemplate template)
    {
        await _dbContext.PromotionViews.InsertOneAsync(template);
    }

    public async Task<List<PromotionViewTemplate>> GetPromotionViewTemplates()
    {
        return await _dbContext.PromotionViews.Find(_ => true).ToListAsync();
    }

    public async Task<PromotionViewTemplate?> GetPromotionViewTemplateByIdAsync(ObjectId id)
    {
        var filter = Builders<PromotionViewTemplate>.Filter.Eq(ct => ct.Id, id);
        return await _dbContext.PromotionViews.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<PromotionViewTemplate?> UpdatePromotionViewTemplateAsync(ObjectId id, PromotionViewTemplate updatedCoinTemplate)
    {
        var filter = Builders<PromotionViewTemplate>.Filter.Eq(ct => ct.Id, id);
        var result = await _dbContext.PromotionViews.ReplaceOneAsync(filter, updatedCoinTemplate);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return updatedCoinTemplate;
        }

        return null;
    }
}
