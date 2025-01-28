namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionTemplateService
{
    Task<ApplicationResult<PaginatedResult<PromotionTemplateListDto>>> GetAllPromotionTemplates(BaseFilter filter);
    Task<ApplicationResult<PromotionTemplateListDto>> GetPromotionTemplateById(string id);
    Task<ApplicationResult<bool>> CreatePromotionTemplate(CreatePromotionTemplate template);
    Task<ApplicationResult<bool>> DeletePromotionTemplate(string id);
}
