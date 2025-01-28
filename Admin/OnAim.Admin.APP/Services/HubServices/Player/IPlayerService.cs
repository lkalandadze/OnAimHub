using OnAim.Admin.APP.Services.Hub.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Transaction;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.APP.Services.HubServices.Player;

public interface IPlayerService
{
    Task<ApplicationResult<bool>> BanPlayer(int playerId, DateTimeOffset? expireDate, bool isPermanent, string description);
    Task<ApplicationResult<bool>> RevokeBan(int id);
    Task<ApplicationResult<bool>> UpdateBan(int id, DateTimeOffset? expireDate, bool isPermanent, string description);
    Task<ApplicationResult<PaginatedResult<PlayerListDto>>> GetAll(PlayerFilter filter);
    Task<ApplicationResult<PlayerDto>> GetById(int id);
    Task<ApplicationResult<List<PlayerBalanceDto>>> GetBalance(int id);

    Task<ApplicationResult<bool>> AddBalanceToPlayer(AddBalanceDto command);
    Task<ApplicationResult<PlayerBan>> GetBannedPlayer(int id);
    Task<ApplicationResult<List<BannedPlayerListDto>>> GetAllBannedPlayers();
    Task<ApplicationResult<List<LeaderboardResult>>> GetLeaderBoardResultByPlayer(int playerId);
    Task<ApplicationResult<PlayerProgressDto>> GetPlayerProgress(int id);
    Task<ApplicationResult<PaginatedResult<PlayerTransactionDto>>> GetPlayerTransaction(int id, BaseFilter filter);
    Task<ApplicationResult<PaginatedResult<PlayerLogDto>>> GetPlayerLogs(int id, BaseFilter filter);
}