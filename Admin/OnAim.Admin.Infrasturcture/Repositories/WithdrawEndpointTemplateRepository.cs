using MongoDB.Bson;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.Infrasturcture.Repositories;

public class WithdrawEndpointTemplateRepository : IWithdrawEndpointTemplateRepository
{
    private readonly AuditLogDbContext _dbContext;

    public WithdrawEndpointTemplateRepository(AuditLogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddWithdrawEndpointTemplateAsync(WithdrawEndpointTemplate template)
    {
        await _dbContext.WithdrawEndpointTemplates.InsertOneAsync(template);
    }

    public async Task<List<WithdrawEndpointTemplate>> GetWithdrawEndpointTemplates()
    {
        return await _dbContext.WithdrawEndpointTemplates.Find(_ => true).ToListAsync();
    }

    public async Task<WithdrawEndpointTemplate?> GetWithdrawEndpointTemplateByIdAsync(ObjectId id)
    {
        var filter = Builders<WithdrawEndpointTemplate>.Filter.Eq(ct => ct.Id, id);
        return await _dbContext.WithdrawEndpointTemplates.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<WithdrawEndpointTemplate?> UpdateWithdrawEndpointTemplateAsync(ObjectId id, WithdrawEndpointTemplate updatedWithdrawEndpointTemplate)
    {
        var filter = Builders<WithdrawEndpointTemplate>.Filter.Eq(ct => ct.Id, id);
        var result = await _dbContext.WithdrawEndpointTemplates.ReplaceOneAsync(filter, updatedWithdrawEndpointTemplate);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return updatedWithdrawEndpointTemplate;
        }

        return null;
    }
}
