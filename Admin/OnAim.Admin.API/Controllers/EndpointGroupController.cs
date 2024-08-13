using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Attributes;
using OnAim.Admin.APP.Commands.EndpointGroup.Create;
using OnAim.Admin.APP.Commands.EndpointGroup.Update;
using OnAim.Admin.APP.Models;
using OnAim.Admin.APP.Queries.EndpointGroup.GetAll;
using OnAim.Admin.APP.Queries.EndpointGroup.GetById;
using System.Net;

namespace OnAim.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndpointGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EndpointGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        [Permission("EndpointGroup/Create")]
        [ProducesResponseType(typeof(CreateEndpointGroupCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEndpointGroupCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Permission("EndpointGroup/Update")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateEndpointGroupModel model)
        {
            var command = new UpdateEndpointGroupCommand(id, model);
            return Ok(await _mediator.Send(command));
        }

        [Permission("EndpointGroup/GetAll")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllEndpointGroupQuery()));
        }

        [Permission("EndpointGroup/Get")]
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await _mediator.Send(new GetEndpointGroupByIdQuery(id)));
        }
    }
}
