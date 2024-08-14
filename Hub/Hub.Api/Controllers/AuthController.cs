using Hub.Application.Features.IdentityFeatures.Commands.CreateAuthenticationToken;
using Hub.IntegrationEvents;
using Hub.Shared.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEvents.IntegrationEvents;
using System.Net.Http;
using System.Text;

namespace Hub.Api.Controllers;

[Route("hubapi/[controller]")]
[ApiController]
public class AuthController : BaseApiController
{
    private readonly IIntegrationEventService _integrationEventService;
    private readonly IBus _bus;

    public AuthController(IIntegrationEventService integrationEventService, IBus bus)
    {
        _integrationEventService = integrationEventService;
        _bus = bus;
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

    [HttpPost("spin-wheel")]
    public async Task<IActionResult> SpinWheel(Guid userId)
    {
        var correlationId = Guid.NewGuid();
        var spinEvent = new SpinRequestedIntegrationEvent(correlationId, userId);

        await _bus.Publish(spinEvent);

        return Ok(new { Message = "Spin request sent successfully!", CorrelationId = correlationId });
    }
}
