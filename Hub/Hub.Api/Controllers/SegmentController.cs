using Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;
using Hub.Application.Features.SegmentFeatures.Commands.DeleteSegment;
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

    [HttpPut]
    public async Task<IActionResult> UpdateSegmentAsync([FromBody] UpdateSegmentCommand command)
    {
        _ = await Mediator.Send(command);

        return StatusCode(200);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        _ = await Mediator.Send(new DeleteSegmentCommand(id));

        return Ok();
    }
}