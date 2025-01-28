using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionViewTemplateService
{
    Task<ApplicationResult<PaginatedResult<PromotionViewTemplate>>> GetAllPromotionViewTemplates(BaseFilter filter);
    Task<ApplicationResult<PromotionViewTemplate>> GetPromotionViewTemplateById(string id);
    Task<ApplicationResult<bool>> CreatePromotionViewTemplateAsync(CreatePromotionViewTemplateAsyncDto create);
    Task<ApplicationResult<bool>> DeletePromotionViewTemplate(string id);
}
