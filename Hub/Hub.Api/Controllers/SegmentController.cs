using Hub.Api.Models.Segments;
using Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;
using Hub.Application.Features.SegmentFeatures.Commands.DeleteSegment;
using Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayers;
using Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayer;
using Hub.Application.Features.SegmentFeatures.Commands.UpdateSegment;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

public class SegmentController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateSegmentAsync([FromBody] CreateSegmentCommand command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPost("{id}/AssignPlayer/{playerId}")]
    public async Task<IActionResult> AssignPlayerAsync(string id, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new AssignSegmentToPlayerCommand(id, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost("{id}/UnassignPlayer/{playerId}")]
    public async Task<IActionResult> UnassignPlayerAsync(string id, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new UnassignSegmentToPlayerCommand(id, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost("{id}/AssignPlayers")]
    public async Task<IActionResult> AssignPlayersAsync(string id, [FromForm] PlayersSegmentRequestModel request)
    {
        var command = new AssignSegmentToPlayersCommand(id, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost("{id}/UnassignPlayers")]
    public async Task<IActionResult> UnassignPlayersAsync(string id, [FromForm] PlayersSegmentRequestModel request)
    {
        var command = new UnassignSegmentToPlayersCommand(id, request.File, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost("{id}/BlockPlayer/{playerId}")]
    public async Task<IActionResult> BlockPlayerAsync(string id, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new BlockSegmentForPlayerCommand(id, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPost("{id}/UnblockPlayer/{playerId}")]
    public async Task<IActionResult> UnblockPlayer(string id, int playerId, [FromForm] PlayerSegmentRequestModel request)
    {
        var command = new UnblockSegmentForPlayerCommand(id, playerId, request.ByUserId);
        var result = await Mediator.Send(command);

        return StatusCode(200, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSegment([FromBody] UpdateSegmentCommand command)
    {
        _ = await Mediator.Send(command);

        return StatusCode(200);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        _ = await Mediator.Send(new DeleteSegmentCommand(id));

        return Ok();
    }
}