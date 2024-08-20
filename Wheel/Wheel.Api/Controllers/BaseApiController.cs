using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wheel.Api.Controllers;

[Authorize]
[ApiController]
[Route("wheelapi/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    
}