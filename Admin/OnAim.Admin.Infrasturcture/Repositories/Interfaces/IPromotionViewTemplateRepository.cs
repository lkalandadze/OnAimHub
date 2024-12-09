using MongoDB.Bson;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Repositories.Abstract;

public interface IPromotionViewTemplateRepository
{
    Task AddPromotionViewTemplateAsync(PromotionViewTemplate template);
    Task<List<PromotionViewTemplate>> GetPromotionViewTemplates();
    Task<PromotionViewTemplate?> GetPromotionViewTemplateByIdAsync(string id);
    Task<PromotionViewTemplate?> UpdatePromotionViewTemplateAsync(string id, PromotionViewTemplate updatedCoinTemplate);
}
