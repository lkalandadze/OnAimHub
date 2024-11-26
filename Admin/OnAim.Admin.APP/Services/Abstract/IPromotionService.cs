using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IPromotionService
{
    Task<ApplicationResult> GetAllPromotions(PromotionFilter baseFilter);
    Task<ApplicationResult> GetPromotionById(int id);
    Task<ApplicationResult> CreatePromotion(Admin.Domain.CreatePromotionDto create);
    Task<ApplicationResult> CreatePromotionView(CreatePromotionView create);
    Task<ApplicationResult> CreatePromotionViewTemplate(CreatePromotionViewTemplate create);
    Task<ApplicationResult> CreateCoinTemplate(CreateCoinTemplate create);
    Task<ApplicationResult> UpdateCoinTemplate(UpdateCoinTemplate update);
    Task<ApplicationResult> DeleteCoinTemplate(DeleteCoinTemplate delete);
}
