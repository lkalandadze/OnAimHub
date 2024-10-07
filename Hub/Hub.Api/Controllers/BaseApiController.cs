#nullable disable

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

//[Authorize]
[ApiController]
[Route("HubApi/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}