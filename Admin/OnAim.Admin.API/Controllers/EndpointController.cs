﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;
using OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetById;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Validation;
using OnAim.Admin.Contracts.Dtos.Endpoint;
using OnAim.Admin.Contracts.Paging;
using System.Net;

namespace OnAim.Admin.API.Controllers;

public class EndpointController : ApiControllerBase
{
    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(CreateEndpointCommand), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Contracts.ApplicationInfrastructure.Error), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ApplicationResult<string>>> Create([FromBody] CreateEndpointCommand command)
         => Ok(await Mediator.Send(command));

    [HttpPut(nameof(Update) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<string>>> Update([FromRoute] int id, [FromBody] UpdateEndpointDto model)
        => Ok(await Mediator.Send(new UpdateEndpointCommand { Id = id, Endpoint = model }));

    [HttpGet(nameof(GetAll))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<EndpointResponseModel>>>> GetAll([FromQuery] EndpointFilter filter)
        => Ok(await Mediator.Send(new GetAllEndpointQuery(filter)));

    [HttpGet(nameof(Get) + "/{id}")]
    public async Task<ActionResult<ApplicationResult<EndpointResponseModel>>> Get([FromRoute] int id)
        => Ok(await Mediator.Send(new GetEndpointByIdQuery(id)));

    [HttpPost(nameof(Delete))]
    public async Task<ActionResult<ApplicationResult<bool>>> Delete([FromBody] List<int> ids)
        => Ok(await Mediator.Send(new DeleteEndpointCommand(ids)));
}
