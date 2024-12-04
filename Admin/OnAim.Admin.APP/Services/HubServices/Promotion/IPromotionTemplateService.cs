using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionTemplateService
{
    Task<ApplicationResult> GetAllTemplates();
    Task<ApplicationResult> GetById(string id);
    Task<ApplicationResult> CreatePromotionTemplate(CreatePromotionTemplate template);
}
