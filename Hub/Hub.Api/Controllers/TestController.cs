using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEvents.IntegrationEvents;

namespace Hub.Api.Controllers;

public class TestController : BaseApiController
{
    private readonly IBus _bus;

    public TestController(IBus bus)
    {
        _bus = bus;
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