using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.BanPlayer;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.RevokePlayerBan;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.UpdatePlayerBan;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayer;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayers;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetProgress;
using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.API.Controllers;

public class PlayerController : ApiControllerBase
{
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] PlayerFilter filter)
        => Ok(await Mediator.Send(new GetAllPlayerQuery(filter)));

    [HttpGet(nameof(GetById) + "/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
        => Ok(await Mediator.Send(new GetPlayerByIdQuery(id)));

    [HttpGet(nameof(GetPlayerBalance) + "/{id}")]
    public async Task<IActionResult> GetPlayerBalance([FromRoute] int id)
        => Ok(await Mediator.Send(new GetPlayerByIdQuery(id)));

    [HttpGet(nameof(GetPlayerProgress) + "/{id}")]
    public async Task<IActionResult> GetPlayerProgress([FromRoute] int id)
        => Ok(await Mediator.Send(new GetPlayerProgressQuery(id)));

    [HttpGet(nameof(GetBannedPlayers))]
    public async Task<IActionResult> GetBannedPlayers()
        => Ok(await Mediator.Send(new GetBannedPlayersQuery()));

    [HttpGet(nameof(GetBannedPlayer) + "/{id}")]
    public async Task<IActionResult> GetBannedPlayer([FromRoute] int id)
        => Ok(await Mediator.Send(new GetBannedPlayerQuery(id)));

    [HttpPost(nameof(BanPlayer))]
    public async Task<IActionResult> BanPlayer([FromBody] BanPlayerCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPut(nameof(RevokePlayerBan))]
    public async Task<IActionResult> RevokePlayerBan([FromBody] RevokePlayerBanCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPut(nameof(UpdateBannedPlayer))]
    public async Task<IActionResult> UpdateBannedPlayer([FromBody] UpdatePlayerBanCommand command)
        => Ok(await Mediator.Send(command));
}
