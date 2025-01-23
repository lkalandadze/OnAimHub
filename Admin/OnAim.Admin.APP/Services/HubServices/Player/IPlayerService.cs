using OnAim.Admin.APP.Services.Hub.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Services.HubServices.Player;

public interface IPlayerService
{
    Task<ApplicationResult> BanPlayer(int playerId, DateTimeOffset? expireDate, bool isPermanent, string description);
    Task<ApplicationResult> RevokeBan(int id);
    Task<ApplicationResult> UpdateBan(int id, DateTimeOffset? expireDate, bool isPermanent, string description);
    Task<ApplicationResult> GetAll(PlayerFilter filter);
    Task<ApplicationResult> GetById(int id);
    Task<ApplicationResult> GetBalance(int id);

    Task<ApplicationResult> AddBalanceToPlayer(AddBalanceDto command);
    Task<ApplicationResult> GetBannedPlayer(int id);
    Task<ApplicationResult> GetAllBannedPlayers();
    Task<ApplicationResult> GetLeaderBoardResultByPlayer(int playerId);
    Task<ApplicationResult> GetPlayerProgress(int id);
    Task<ApplicationResult> GetPlayerTransaction(int id, BaseFilter filter);
    Task<ApplicationResult> GetPlayerLogs(int id, BaseFilter filter);
}