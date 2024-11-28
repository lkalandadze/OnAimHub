using MongoDB.Bson;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Repositories.Abstract;

public interface ICoinRepository
{
    Task AddCoinTemplateAsync(CoinTemplate coinTemplate);
    Task<List<CoinTemplate>> GetCoinTemplates();
    Task<CoinTemplate?> GetCoinTemplateByIdAsync(ObjectId id);
    Task<CoinTemplate?> UpdateCoinTemplateAsync(ObjectId id, CoinTemplate updatedCoinTemplate);
}
