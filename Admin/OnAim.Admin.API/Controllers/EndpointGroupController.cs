using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Create;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetById;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Validation;
using System.Net;

namespace OnAim.Admin.API.Controllers;

public class EndpointGroupController : ApiControllerBase
{
    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(CreateEndpointGroupCommand), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEndpointGroupCommand command)
       => Ok(await Mediator.Send(command));

    [HttpPut(nameof(Update) + "/{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEndpointGroupRequest model)
        => Ok(await Mediator.Send(new UpdateEndpointGroupCommand(id, model)));

    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] EndpointGroupFilter filter)
        => Ok(await Mediator.Send(new GetAllEndpointGroupQuery(filter)));

    [HttpGet(nameof(Get) + "/{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
        => Ok(await Mediator.Send(new GetEndpointGroupByIdQuery(id)));

    [HttpPost(nameof(Delete))]
    public async Task<IActionResult> Delete([FromBody] List<int> ids)
        => Ok(await Mediator.Send(new DeleteEndpointGroupCommand(ids)));
}
