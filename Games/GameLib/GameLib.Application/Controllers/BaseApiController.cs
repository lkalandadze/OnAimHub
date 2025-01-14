#nullable disable

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GameLib.Application.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{

    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}