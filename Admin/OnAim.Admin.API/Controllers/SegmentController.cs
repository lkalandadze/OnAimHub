using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById;

namespace OnAim.Admin.API.Controllers
{
    public class SegmentController : ApiControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllSegmentQuery query)
            => Ok(await Mediator.Send(query));

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
            => Ok(await Mediator.Send(new GetSegmentByIdQuery(id)));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateSegmentCommand command)
            => Ok(await Mediator.Send(command));
    }
}
