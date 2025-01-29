using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.AddBalanceToPlayer;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.BanPlayer;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.RevokePlayerBan;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.UpdatePlayerBan;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBalance;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayer;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayers;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetProgress;
using OnAim.Admin.APP.Services.Hub.Player;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Transaction;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.API.Controllers;

public class PlayerController : ApiControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet(nameof(GetAll))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<PlayerListDto>>>> GetAll([FromQuery] PlayerFilter filter)
        => Ok(await Mediator.Send(new GetAllPlayerQuery(filter)));

    [HttpGet(nameof(GetById) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<PlayerDto>>> GetById([FromRoute] int id)
        => Ok(await Mediator.Send(new GetPlayerByIdQuery(id)));

    [HttpGet(nameof(GetPlayerBalance) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<List<PlayerBalanceDto>>>> GetPlayerBalance([FromRoute] int id)
        => Ok(await Mediator.Send(new GetPlayerBalanceQuery(id)));

    [HttpPost(nameof(AddBalanceToPlayer))]
    public async Task<ActionResult<ApplicationResult<bool>>> AddBalanceToPlayer([FromBody] AddBalanceDto command)
        => Ok(await Mediator.Send(new AddBalanceToPlayerCommand(command)));

    [HttpGet(nameof(GetPlayerProgress) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<PlayerProgressDto>>> GetPlayerProgress([FromRoute] int id)
        => Ok(await Mediator.Send(new GetPlayerProgressQuery(id)));

    [HttpGet(nameof(GetLeaderBoardResultByPlayerId) + "/{id}")]
    public async Task<ActionResult<UserActiveLeaderboards>> GetLeaderBoardResultByPlayerId([FromRoute] int id)
        => Ok(await Mediator.Send(new GetLeaderBoardResultByPlayerIdQuery(id)));

    [HttpGet(nameof(GetPlayerTransaction) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<PlayerTransactionDto>>>> GetPlayerTransaction([FromRoute] int id, [FromQuery] BaseFilter filter)
        => Ok(await _playerService.GetPlayerTransaction(id, filter));

    [HttpGet(nameof(GetPlayerLogs) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<PlayerLogDto>>>> GetPlayerLogs([FromRoute] int id, [FromQuery] BaseFilter filter)
        => Ok(await _playerService.GetPlayerLogs(id, filter));

    [HttpGet(nameof(GetBannedPlayers))]
    public async Task<ActionResult<ApplicationResult<List<BannedPlayerListDto>>>> GetBannedPlayers()
        => Ok(await Mediator.Send(new GetBannedPlayersQuery()));

    [HttpGet(nameof(GetBannedPlayer) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<PlayerBan>>> GetBannedPlayer([FromRoute] int id)
        => Ok(await Mediator.Send(new GetBannedPlayerQuery(id)));

    [HttpPost(nameof(BanPlayer))]
    public async Task<ActionResult<ApplicationResult<bool>>> BanPlayer([FromBody] BanPlayerCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPut(nameof(RevokePlayerBan))]
    public async Task<ActionResult<ApplicationResult<bool>>> RevokePlayerBan([FromBody] int id)
        => Ok(await Mediator.Send(new RevokePlayerBanCommand(id)));

    [HttpPut(nameof(UpdateBannedPlayer))]
    public async Task<ActionResult<ApplicationResult<bool>>> UpdateBannedPlayer([FromBody] UpdatePlayerBanCommand command)
        => Ok(await Mediator.Send(command));
}
