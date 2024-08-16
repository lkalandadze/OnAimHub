using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEvents.IntegrationEvents;

namespace Hub.Api.Controllers;

[Route("hubapi/[controller]")]
[ApiController]
public class AuthController : BaseApiController
{
    private readonly IBus _bus;

    public AuthController(IBus bus)
    {
        _bus = bus;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<CreateAuthenticationTokenResponse>> Auth(CreateAuthenticationTokenRequest request)
    {
        var result = await Mediator.Send(request);

        return !result.Success ? StatusCode(401) : StatusCode(200, result);
    }

    [HttpPost("spin-wheel")]
    public async Task<IActionResult> SpinWheel(Guid userId)
    {
        var correlationId = Guid.NewGuid();
        var spinEvent = new SpinRequestedIntegrationEvent(correlationId, userId);

        await _bus.Publish(spinEvent);

        return Ok(new { Message = "Spin request sent successfully!", CorrelationId = correlationId });
    }
}
