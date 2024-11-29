using MongoDB.Bson;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Repositories.Abstract;

public interface ICoinRepository
{
    Task AddCoinTemplateAsync(CoinTemplate coinTemplate);
    Task<List<CoinTemplate>> GetCoinTemplates();
    Task<CoinTemplate?> GetCoinTemplateByIdAsync(string id);
    Task<CoinTemplate?> UpdateCoinTemplateAsync(string id, CoinTemplate updatedCoinTemplate);
}
