﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayersToSegment;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayersToSegment;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockPlayer;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockSegmentForPlayers;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.Update;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.Acts;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.History;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetGeneralSegmentActsHistory;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetSegmentActs;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.API.Controllers
{
    public class SegmentController : ApiControllerBase
    {
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

        [HttpGet(nameof(GetSegmentActsById) + "/{segmentId}")]
        public async Task<IActionResult> GetSegmentActsById([FromRoute] string segmentId)
            => Ok(await Mediator.Send(new GetSegmentActsByIdQuery(segmentId)));

        [HttpGet(nameof(GetSegmentActsHistoryById) + "/{playerSegmentActId}")]
        public async Task<IActionResult> GetSegmentActsHistoryById([FromRoute] int playerSegmentActId)
            => Ok(await Mediator.Send(new GetSegmentActsHistoryByIdQuery(playerSegmentActId)));

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] CreateSegmentCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPut(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] UpdateSegmentCommand command)
            => Ok(await Mediator.Send(command));

        [HttpDelete(nameof(Delete) + "/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
            => Ok(await Mediator.Send(new DeleteSegmentCommand(id)));

        [HttpPost(nameof(AssignPlayerToSegment))]
        public async Task<IActionResult> AssignPlayerToSegment([FromBody] AssignPlayerCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost(nameof(UnAssignPlayer))]
        public async Task<IActionResult> UnAssignPlayer([FromBody] UnAssignPlayerCommand command)
            => Ok(await Mediator.Send(command));
 
        [HttpPost(nameof(AssignSegmentToPlayers))]
        public async Task<IActionResult> AssignSegmentToPlayers(IFormFile file, [FromBody] AssignSegmentToPlayersCommand command)
            => Ok(await Mediator.Send(command with { File = file}));

        [HttpPost(nameof(UnAssignPlayersToSegment))]
        public async Task<IActionResult> UnAssignPlayersToSegment(IFormFile file, [FromBody] UnAssignPlayersToSegmentCommand command)
            => Ok(await Mediator.Send(command with { File = file }));

        [HttpPost(nameof(BlockSegmentForPlayer))]
        public async Task<IActionResult> BlockSegmentForPlayer([FromBody] BlockSegmentForPlayerCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost(nameof(UnBlockSegmentForPlayer))]
        public async Task<IActionResult> UnBlockSegmentForPlayer([FromBody] UnBlockSegmentForPlayerCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost(nameof(BlockSegmentForPlayers))]
        public async Task<IActionResult> BlockSegmentForPlayers(IFormFile file, [FromBody] BlockSegmentForPlayersCommand command)
            => Ok(await Mediator.Send(command with { File = file }));

        [HttpPost(nameof(UnBlockSegmentForPlayers))]
        public async Task<IActionResult> UnBlockSegmentForPlayers(IFormFile file, [FromBody] UnBlockSegmentForPlayersCommand command)
            => Ok(await Mediator.Send(command with { File = file }));
    }
}
