using OnAim.Admin.APP.Services.Hub.Promotion;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public interface IPromotionService
{
    Task<ApplicationResult<bool>> DeletePromotion(int id);
    Task<ApplicationResult<Guid>> CreatePromotion(CreatePromotionDto create);
    Task<ApplicationResult<object>> CreatePromotionView(CreatePromotionView create);
    Task<ApplicationResult<bool>> UpdatePromotionStatus(UpdatePromotionStatusDto update);

    Task<ApplicationResult<List<Service>>> GetAllService();
    Task<ApplicationResult<PromotionDto>> GetPromotionById(int id);
    Task<ApplicationResult<PaginatedResult<PromotionDto>>> GetAllPromotions(PromotionFilter baseFilter);
    Task<object> GetAllPromotionGames(int promotionId, BaseFilter filter);
    Task<ApplicationResult<PaginatedResult<PlayerListDto>>> GetPromotionPlayers(int promotionId, PlayerFilter filter);
    Task<ApplicationResult<PromotionLeaderboardDto<object>>> GetPromotionLeaderboards(int promotionId, BaseFilter filter);
    Task<ApplicationResult<PaginatedResult<PromotionLeaderboardDetailDto>>> GetPromotionLeaderboardDetails(int leaderboardId, BaseFilter filter);
    Task<ApplicationResult<PaginatedResult<PlayerTransactionDto>>> GetPromotionPlayerTransaction(int playerId, PlayerTransactionFilter filter);


    //delete later
    Task<PromotionResponse> CreatePromotionAsync(CreatePromotionCommandDto request);
    Task<int> CreateLeaderboardRecordAsync(CreateLeaderboardRecord leaderboard);
    Task<ApplicationResult<object>> CreateGameConfiguration(string name, object configurationJson);
    Task<ApplicationResult<object>> CreateAggregationConfiguration(List<AggregationConfiguration> configuration);
    Task CompensateAsync(Guid request, string? gameName);
}
