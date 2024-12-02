using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IPromotionTemplateService
{
    Task<ApplicationResult> GetAllTemplates();
    Task<ApplicationResult> GetById(string id);
    Task<ApplicationResult> CreatePromotionTemplate(PromotionTemplate template);
}
