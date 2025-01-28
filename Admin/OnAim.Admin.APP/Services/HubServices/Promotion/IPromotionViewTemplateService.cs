namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionViewTemplateService
{
    Task<ApplicationResult<PaginatedResult<PromotionViewTemplate>>> GetAllPromotionViewTemplates(BaseFilter filter);
    Task<ApplicationResult<PromotionViewTemplate>> GetPromotionViewTemplateById(string id);
    Task<ApplicationResult<bool>> CreatePromotionViewTemplateAsync(CreatePromotionViewTemplateAsyncDto create);
    Task<ApplicationResult<bool>> DeletePromotionViewTemplate(string id);
}
