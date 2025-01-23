using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionService
{
    Task<ApplicationResult> DeletePromotion(int id);
    Task<ApplicationResult> CreatePromotion(CreatePromotionDto create);
    Task<ApplicationResult> CreatePromotionView(CreatePromotionView create);
    Task<ApplicationResult> UpdatePromotionStatus(UpdatePromotionStatusDto update);

    Task<ApplicationResult> GetAllService();
    Task<ApplicationResult> GetPromotionById(int id);
    Task<ApplicationResult> GetAllPromotions(PromotionFilter baseFilter);
    Task<object> GetAllPromotionGames(int promotionId, BaseFilter filter);
    Task<ApplicationResult> GetPromotionPlayers(int promotionId, PlayerFilter filter);
    Task<ApplicationResult> GetPromotionLeaderboards(int promotionId, BaseFilter filter);
    Task<ApplicationResult> GetPromotionLeaderboardDetails(int leaderboardId, BaseFilter filter);
    Task<ApplicationResult> GetPromotionPlayerTransaction(int playerId, PlayerTransactionFilter filter);
}
