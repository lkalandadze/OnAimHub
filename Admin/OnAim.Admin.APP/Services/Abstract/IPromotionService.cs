using MongoDB.Bson;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Domain.Interfaces;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IPromotionService
{
    Task<ApplicationResult> GetAllPromotions(PromotionFilter baseFilter);
    Task<ApplicationResult> GetPromotionById(ObjectId id);
    Task<ApplicationResult> CreatePromotion(CreatePromotionDto create);
    Task<ApplicationResult> UpdatePromotion(ObjectId promotionId, OnAim.Admin.Domain.HubEntities.Promotion updatedPromotion);
}
