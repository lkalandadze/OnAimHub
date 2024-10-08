﻿using Hub.Api.Models.Segments;
using Hub.Application.Features.GameFeatures.Queries.GetAllGame;
using Hub.Application.Features.PlayerBanFeatures.Commands.Create;
using Hub.Application.Features.PlayerBanFeatures.Commands.Revoke;
using Hub.Application.Features.PlayerBanFeatures.Commands.Update;
using Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;
using Hub.Application.Features.SegmentFeatures.Commands.DeleteSegment;
using Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.UpdateSegment;
using Hub.Application.Models.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[Authorize(AuthenticationSchemes = "BasicAuthentication")]
[ApiExplorerSettings(GroupName = "admin")]
public class AdminController : BaseApiController
{
    #region Games

    [HttpGet(nameof(Games))]
    public async Task<ActionResult<IEnumerable<ActiveGameModel>>> Games()
    {
        return Ok(await Mediator.Send(new GetAllGameQuery(false)));
    }

    #endregion

    #region Players

    [HttpPut(nameof(UpdateBannedPlayer))]
    public async Task<ActionResult> UpdateBannedPlayer(UpdatePlayerBanCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPut(nameof(RevokePlayerBan))]
    public async Task<ActionResult> RevokePlayerBan(RevokePlayerBanCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPost(nameof(BanPlayer))]
    public async Task<ActionResult> BanPlayer(CreatePlayerBanCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    #endregion

    #region Segments

    [HttpPost(nameof(CreateSegment))]
    public async Task<IActionResult> CreateSegment([FromBody] CreateSegmentCommand command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPost(nameof(AssignSegmentToPlayer))]
    public async Task<IActionResult> AssignSegmentToPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new AssignSegmentToPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnassignSegmentToPlayer))]
    public async Task<IActionResult> UnassignSegmentToPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new UnassignSegmentToPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(AssignSegmentToPlayers))]
    public async Task<IActionResult> AssignSegmentToPlayers(string segmentId, [FromForm] PlayersSegmentRequestModel request)
    {
        var command = new AssignSegmentToPlayersCommand(segmentId, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnassignSegmentToPlayers))]
    public async Task<IActionResult> UnassignSegmentToPlayers(string segmentId, [FromForm] PlayersSegmentRequestModel request)
    {
        var command = new UnassignSegmentToPlayersCommand(segmentId, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(BlockSegmentForPlayer))]
    public async Task<IActionResult> BlockSegmentForPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new BlockSegmentForPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnblockSegmentForPlayer))]
    public async Task<IActionResult> UnblockSegmentForPlayer(string segmentId, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new UnblockSegmentForPlayerCommand(segmentId, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(BlockSegmentForPlayers))]
    public async Task<IActionResult> BlockSegmentForPlayers(string segmentId, [FromForm] PlayersSegmentRequestModel request)
    {
        var command = new BlockSegmentForPlayersCommand(segmentId, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost(nameof(UnblockSegmentForPlayers))]
    public async Task<IActionResult> UnblockSegmentForPlayers(string segmentId, [FromForm] PlayersSegmentRequestModel request)
    {
        var command = new UnblockSegmentForPlayersCommand(segmentId, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPut(nameof(UpdateSegment))]
    public async Task<IActionResult> UpdateSegment([FromBody] UpdateSegmentCommand command)
    {
        _ = await Mediator.Send(command);

        return StatusCode(200);
    }

    [HttpDelete(nameof(DeleteSegment))]
    public async Task<IActionResult> DeleteSegment(string id)
    {
        _ = await Mediator.Send(new DeleteSegmentCommand(id));

        return Ok();
    }

    #endregion
}