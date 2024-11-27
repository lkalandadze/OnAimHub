using MongoDB.Bson;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IPromotionViewTemplateService
{
    Task<ApplicationResult> GetAllWithdrawEndpointTemplates();
    Task<ApplicationResult> GetById(ObjectId id);
    Task<ApplicationResult> CreatePromotionViewTemplateAsync(CreatePromotionViewTemplateAsyncDto create);
}
