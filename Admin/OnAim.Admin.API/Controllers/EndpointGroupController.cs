using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Commands.EndpointGroup.Create;
using OnAim.Admin.APP.Commands.EndpointGroup.Delete;
using OnAim.Admin.APP.Commands.EndpointGroup.Update;
using OnAim.Admin.APP.Queries.EndpointGroup.EndpointGroupsExport;
using OnAim.Admin.APP.Queries.EndpointGroup.GetAll;
using OnAim.Admin.APP.Queries.EndpointGroup.GetById;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using System.Net;

namespace OnAim.Admin.API.Controllers
{
    public class EndpointGroupController : ApiControllerBase
    {
        [HttpPost("Create")]
        [ProducesResponseType(typeof(CreateEndpointGroupCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEndpointGroupCommand command)
           => Ok(await Mediator.Send(command));

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEndpointGroupRequest model)
            => Ok(await Mediator.Send(new UpdateEndpointGroupCommand(id, model)));

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] EndpointGroupFilter filter)
            => Ok(await Mediator.Send(new GetAllEndpointGroupQuery(filter)));

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
            => Ok(await Mediator.Send(new GetEndpointGroupByIdQuery(id)));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
            => Ok(await Mediator.Send(new DeleteEndpointGroupCommand(id)));

        //[HttpGet("Download")]
        //public async Task<IResult> Download([FromQuery] EndpointGroupsExportQuery query)
        //{
        //    var result = await Mediator.Send(query);
        //    return result;
        //}
    }
}
