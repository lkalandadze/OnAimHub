using Hub.Application.Features.IdentityFeatures.Commands.ApplyPromoCode;
using Hub.Application.Features.PlayerBanFeatures.Commands.Create;
using Hub.Application.Features.PlayerBanFeatures.Commands.Revoke;
using Hub.Application.Features.PlayerBanFeatures.Commands.Update;
using Hub.Application.Features.PlayerBanFeatures.Queries.Get;
using Hub.Application.Features.PlayerBanFeatures.Queries.GetById;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayerBalance;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayerProgress;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;
using Hub.Application.Features.PlayerFeatures.Queries.GetPromoCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[AllowAnonymous]
public class PlayerController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<GetPlayersResponse>> GetAllAsync([FromQuery] int? page, int? pageSize)
    {
        var query = new GetPlayersQuery
        {
            PageNumber = page,
            PageSize = pageSize,
        };

        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPlayerResponse>> GetAsync(int id)
    {
        return Ok(await Mediator.Send(new GetPlayerQuery { PlayerId = id }));
    }

    [HttpGet("Balances")]
    public async Task<ActionResult<GetPlayerBalanceResponse>> GetPlayerBalancesAsync()
    {
        return Ok(await Mediator.Send(new GetPlayerBalanceQuery()));
    }

    [HttpGet("Progress")]
    public async Task<ActionResult<GetPlayerProgressResponse>> GetPlayerProgressAsync()
    {
        return Ok(await Mediator.Send(new GetPlayerProgressQuery()));
    }

    [HttpPost(nameof(ApplyPromoCode))]
    public async Task<Unit> ApplyPromoCode(ApplyPromoCodeCommand request) => await Mediator.Send(request);
    
    [HttpGet(nameof(GetPromoCode))]
    public async Task<string> GetPromoCode([FromQuery] GetPromoCodeQuery request) => await Mediator.Send(request);

    #region Player Bans

    [HttpGet(nameof(GetBannedPlayers))]
    public async Task<ActionResult<GetBannedPlayersQueryResponse>> GetBannedPlayers([FromQuery] GetBannedPlayersQuery request) => await Mediator.Send(request);

    [HttpGet(nameof(GetBannedPlayerById))]
    public async Task<ActionResult<GetBannedPlayerByIdQueryResponse>> GetBannedPlayerById([FromQuery] GetBannedPlayerByIdQuery request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateBannedPlayer))]
    public async Task<Unit> UpdateBannedPlayer(UpdatePlayerBanCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(RevokePlayerBan))]
    public async Task<Unit> RevokePlayerBan(RevokePlayerBanCommand request) => await Mediator.Send(request);

    [HttpPost(nameof(BanPlayer))]
    public async Task<Unit> BanPlayer(CreatePlayerBanCommand request) => await Mediator.Send(request);

    #endregion
}