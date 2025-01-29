using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Create;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetById;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;
using System.Net;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.API.Controllers;

public class EndpointGroupController : ApiControllerBase
{
    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(CreateEndpointGroupCommand), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ApplicationResult<string>>> Create([FromBody] CreateEndpointGroupCommand command)
       => Ok(await Mediator.Send(command));

    [HttpPut(nameof(Update) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<string>>> Update([FromRoute] int id, [FromBody] UpdateEndpointGroupRequest model)
        => Ok(await Mediator.Send(new UpdateEndpointGroupCommand(id, model)));

    [HttpGet(nameof(GetAll))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<EndpointGroupModel>>>> GetAll([FromQuery] EndpointGroupFilter filter)
        => Ok(await Mediator.Send(new GetAllEndpointGroupQuery(filter)));

    [HttpGet(nameof(Get) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<EndpointGroupResponseDto>>> Get([FromRoute] int id)
        => Ok(await Mediator.Send(new GetEndpointGroupByIdQuery(id)));

    [HttpPost(nameof(Delete))]
    public async Task<ActionResult<ApplicationResult<bool>>> Delete([FromBody] List<int> ids)
        => Ok(await Mediator.Send(new DeleteEndpointGroupCommand(ids)));
}
