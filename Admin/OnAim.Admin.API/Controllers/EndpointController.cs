using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;
using OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetById;
using OnAim.Admin.Shared.ApplicationInfrastructure.Validation;
using OnAim.Admin.Shared.DTOs.Endpoint;
using System.Net;

namespace OnAim.Admin.API.Controllers;

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

    //[HttpGet("Download")]
    //public async Task<IResult> Download([FromQuery] EndpointsExportQuery query)
    //{
    //    var result = await Mediator.Send(query);
    //    return result;
    //}

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => Ok(await Mediator.Send(new DeleteEndpointCommand(id)));
}
