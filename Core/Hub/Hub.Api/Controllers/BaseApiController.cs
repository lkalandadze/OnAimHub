#nullable disable

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Hub.Api.Controllers;

//[Authorize]
[ApiController]
[Route("HubApi/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    private JsonSerializerOptions _jsonSerializerOptions;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected JsonSerializerOptions JsonSerializerOptions =>
        _jsonSerializerOptions ??= HttpContext.RequestServices.GetService<JsonSerializerOptions>();
}