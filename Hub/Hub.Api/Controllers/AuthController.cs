using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Microsoft.AspNetCore.Authorization;
using Hub.Application.Models.Auth;
using Hub.Application.Services;
using Hub.IntegrationEvents;
using Hub.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[Route("hubapi/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IIntegrationEventService _integrationEventService;

    public AuthController(IAuthService authService, IIntegrationEventService integrationEventService)
    {
        _authService = authService;
        _integrationEventService = integrationEventService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<CreateAuthenticationTokenResponse>> Auth(CreateAuthenticationTokenRequest request)
    {
        var result = await Mediator.Send(request);

        return !result.Success ? StatusCode(401) : StatusCode(200, result);
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder(Guid userId)
    {
        var correlationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var orderDate = DateTimeOffset.UtcNow;

        var @event = new TestOrderIntegrationEvent(correlationId, orderId, userId, orderDate);

        await _integrationEventService.AddAsync(@event);
        await _integrationEventService.PublishAllAsync();

        return Ok(new { Message = "Order placed successfully!", OrderId = orderId });
    }
}
