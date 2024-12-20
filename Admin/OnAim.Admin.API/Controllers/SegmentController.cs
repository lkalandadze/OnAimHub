using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.Update;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActivePlayers;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.Acts;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.History;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetGeneralSegmentActsHistory;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetSegmentActs;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.API.Controllers;

public class SegmentController : ApiControllerBase
{
    private readonly ISegmentService _segmentService;

    public SegmentController(ISegmentService segmentService)
    {
        _segmentService = segmentService;
    }

    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSegmentQuery query)
        => Ok(await Mediator.Send(query));

    [HttpGet(nameof(GetGeneralSegmentActs))]
    public async Task<IActionResult> GetGeneralSegmentActs([FromQuery] SegmentActsFilter filter)
        => Ok(await Mediator.Send(new GetSegmentActsQuery(filter)));

    [HttpGet(nameof(GetGeneralSegmentActsHistory))]
    public async Task<IActionResult> GetGeneralSegmentActsHistory([FromQuery] SegmentActsFilter filter)
        => Ok(await Mediator.Send(new GetGeneralSegmentActsHistoryQuery(filter)));

    [HttpGet(nameof(GetById) + "/{segmentId}")]
    public async Task<IActionResult> GetById([FromRoute] string segmentId)
        => Ok(await Mediator.Send(new GetSegmentByIdQuery(segmentId)));

    [HttpGet(nameof(GetBlackListedPlayers) + "/{segmentId}")]
    public async Task<IActionResult> GetBlackListedPlayers([FromRoute] string segmentId, [FromQuery] FilterBy filter)
        => Ok(await Mediator.Send(new GetBlackListedPlayersBySegmentIdQuery(segmentId, filter)));

    [HttpGet(nameof(GetActivePlayers) + "/{segmentId}")]
    public async Task<IActionResult> GetActivePlayers([FromRoute] string segmentId, [FromQuery] FilterBy filter)
    => Ok(await Mediator.Send(new GetActivePlayersBySegmentIdQuery(segmentId, filter)));

    [HttpGet(nameof(GetSegmentActsById) + "/{segmentId}")]
    public async Task<IActionResult> GetSegmentActsById([FromRoute] string segmentId)
        => Ok(await Mediator.Send(new GetSegmentActsByIdQuery(segmentId)));

    [HttpGet(nameof(GetSegmentActsHistoryById) + "/{playerSegmentActId}")]
    public async Task<IActionResult> GetSegmentActsHistoryById([FromRoute] int playerSegmentActId)
        => Ok(await Mediator.Send(new GetSegmentActsHistoryByIdQuery(playerSegmentActId)));

    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create([FromBody] CreateSegmentCommand command)
        => Ok(await _segmentService.CreateSegment(command.Id, command.Description, command.PriorityLevel));

    [HttpPut(nameof(Update))]
    public async Task<IActionResult> Update([FromBody] UpdateSegmentCommand command)
        => Ok(await _segmentService.UpdateSegment(command.Id, command.Description, command.PriorityLevel));

    [HttpDelete(nameof(Delete) + "/{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
        => Ok(await _segmentService.DeleteSegment(id));

    [HttpPost(nameof(AssignPlayerToSegment))]
    public async Task<IActionResult> AssignPlayerToSegment([FromBody] AssignPlayerCommand command)
        => Ok(await _segmentService.AssignSegmentToPlayer(command.SegmentId, command.PlayerId));

    [HttpPost(nameof(UnAssignPlayer))]
    public async Task<IActionResult> UnAssignPlayer([FromBody] UnAssignPlayerCommand command)
        => Ok(await _segmentService.UnAssignSegmentForPlayer(command.SegmentId, command.PlayerId));

    [HttpPost(nameof(AssignSegmentToPlayers))]
    public async Task<IActionResult> AssignSegmentToPlayers(IFormFile formFile, [FromForm] IEnumerable<string> segmentId)
    {
        try
        {
            await _segmentService.AssignSegmentToPlayers(segmentId, formFile);
            return Ok(new { results = "File Uploaded Successfully To Database" });
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { results = ve.Message });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
        }
    }

    [HttpPost(nameof(UnAssignPlayersToSegment))]
    public async Task<IActionResult> UnAssignPlayersToSegment(IFormFile formFile, [FromForm] IEnumerable<string> segmentId)
    {
        try
        {
            await _segmentService.UnAssignPlayersToSegment(segmentId, formFile);
            return Ok(new { results = "File Uploaded Successfully To Database" });
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { results = ve.Message });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
        }
    }

    [HttpPost(nameof(BlockSegmentForPlayer))]
    public async Task<IActionResult> BlockSegmentForPlayer([FromBody] BlockSegmentForPlayerCommand command)
        => Ok(await _segmentService.BlockSegmentForPlayer(command.SegmentId, command.PlayerId));

    [HttpPost(nameof(UnBlockSegmentForPlayer))]
    public async Task<IActionResult> UnBlockSegmentForPlayer([FromBody] UnBlockSegmentForPlayerCommand command)
        => Ok(await _segmentService.UnBlockSegmentForPlayer(command.SegmentId, command.PlayerId));

    [HttpPost(nameof(BlockSegmentForPlayers))]
    public async Task<IActionResult> BlockSegmentForPlayers(IFormFile formFile, [FromForm] IEnumerable<string> segmentId)
    {
        try
        {
            await _segmentService.BlockSegmentForPlayers(segmentId, formFile);
            return Ok(new { results = "File Uploaded Successfully To Database" });
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { results = ve.Message });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
        }
    }

    [HttpPost(nameof(UnBlockSegmentForPlayers))]
    public async Task<IActionResult> UnBlockSegmentForPlayers(IFormFile formFile, [FromForm] IEnumerable<string> segmentId)
    {
        try
        {
            await _segmentService.UnBlockSegmentForPlayers(segmentId,formFile);
            return Ok(new { results = "File Uploaded Successfully To Database" });
        }
        catch (ValidationException ve)
        {
            return BadRequest(new { results = ve.Message });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
        }
    }
}
