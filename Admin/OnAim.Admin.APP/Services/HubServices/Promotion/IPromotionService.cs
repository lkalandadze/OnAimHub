using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Promotion;
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

    Task<ApplicationResult> GetAllPromotionGames(int promotionId, BaseFilter filter);
    Task<ApplicationResult> GetPromotionPlayers(int promotionId, PlayerFilter filter);
    Task<ApplicationResult> GetPromotionPlayerTransaction(int playerId, PlayerTransactionFilter filter);
    Task<ApplicationResult> GetPromotionLeaderboards(int promotionId, BaseFilter filter);
    Task<ApplicationResult> GetPromotionLeaderboardDetails(int leaderboardId, BaseFilter filter);
}
