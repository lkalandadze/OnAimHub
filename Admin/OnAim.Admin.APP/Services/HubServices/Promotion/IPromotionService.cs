using OnAim.Admin.Contracts;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Domain;
using OnAim.Admin.Infrasturcture;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionService
{
    Task<ApplicationResult> GetAllPromotions(PromotionFilter baseFilter);
    Task<ApplicationResult> GetPromotionById(int id);
    Task<ApplicationResult> CreatePromotion(CreatePromotionDto create);
    Task<ApplicationResult> CreatePromotionView(CreatePromotionView create);
    Task<ApplicationResult> UpdatePromotionStatus(UpdatePromotionStatusCommand update);
    Task<ApplicationResult> DeletePromotion(SoftDeletePromotionCommand command);
}
