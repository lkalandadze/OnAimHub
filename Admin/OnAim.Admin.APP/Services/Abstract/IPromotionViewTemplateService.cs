using MongoDB.Bson;
using OnAim.Admin.APP.Services.PromotionViewTemplateService;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IPromotionViewTemplateService
{
    Task<ApplicationResult> GetAllWithdrawEndpointTemplates();
    Task<ApplicationResult> GetById(ObjectId id);
    Task<ApplicationResult> CreatePromotionViewTemplateAsync(CreatePromotionViewTemplateAsyncDto create);
}
