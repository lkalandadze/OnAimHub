using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionTemplateService
{
    Task<ApplicationResult> GetAllPromotionTemplates(BaseFilter filter);
    Task<ApplicationResult> GetPromotionTemplateById(string id);
    Task<ApplicationResult> CreatePromotionTemplate(CreatePromotionTemplate template);
    Task<ApplicationResult> DeletePromotionTemplate(string id);
}
