using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Commands.EndPoint.Create;
using OnAim.Admin.APP.Commands.EndPoint.Delete;
using OnAim.Admin.APP.Commands.EndPoint.Update;
using OnAim.Admin.APP.Queries.EndPoint.EndpointsExport;
using OnAim.Admin.APP.Queries.EndPoint.GetAll;
using OnAim.Admin.APP.Queries.EndPoint.GetById;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Net;

namespace OnAim.Admin.API.Controllers
{
    public class EndpointController : ApiControllerBase
    {
        [HttpPost("Create")]
        [ProducesResponseType(typeof(CreateEndpointCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEndpointCommand command)
             => Ok(await Mediator.Send(command));

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEndpointDto model)
            => Ok(await Mediator.Send(new UpdateEndpointCommand { Id = id, Endpoint = model }));

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] EndpointFilter roleFilter)
            => Ok(await Mediator.Send(new GetAllEndpointQuery(roleFilter)));

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
            => Ok(await Mediator.Send(new GetEndpointByIdQuery(id)));

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] EndpointsExportQuery query)
        {
            var result = await Mediator.Send(query);
            return result;
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
            => Ok(await Mediator.Send(new DeleteEndpointCommand(id)));
    }
}
