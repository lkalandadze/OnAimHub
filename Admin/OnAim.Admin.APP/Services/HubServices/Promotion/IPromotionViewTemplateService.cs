using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionViewTemplateService
{
    Task<ApplicationResult> GetAllPromotionViewTemplates(BaseFilter filter);
    Task<ApplicationResult> GetPromotionViewTemplateById(string id);
    Task<ApplicationResult> CreatePromotionViewTemplateAsync(CreatePromotionViewTemplateAsyncDto create);
    Task<ApplicationResult> DeletePromotionViewTemplate(string id);
}
