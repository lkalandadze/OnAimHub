using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Repositories.Interfaces;

public interface IPromotionTemplateRepository
{
    Task AddPromotionTemplateAsync(PromotionTemplate template);
    Task<List<PromotionTemplate>> GetPromotionTemplates();
    Task<PromotionTemplate?> GetPromotionTemplateByIdAsync(string id);
    Task<PromotionTemplate?> UpdatePromotionTemplateAsync(string id, PromotionTemplate updatedCoinTemplate);
}
