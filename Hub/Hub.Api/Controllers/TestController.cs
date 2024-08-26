using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Concrete;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEvents.IntegrationEvents;

namespace Hub.Api.Controllers;

public class TestController : BaseApiController
{
    private readonly IBus _bus;
    private readonly IActiveGameService _activeGameService;
    public TestController(IBus bus, IActiveGameService gameService)
    {
        _bus = bus;
        _activeGameService = gameService;
    }

    [HttpPost("spin-wheel")]
    public async Task<IActionResult> SpinWheel(Guid userId)
    {
        var correlationId = Guid.NewGuid();
        var spinEvent = new SpinRequestedIntegrationEvent(correlationId, userId);

        await _bus.Publish(spinEvent);

        return Ok(new { Message = "Spin request sent successfully!", CorrelationId = correlationId });
    }

    [AllowAnonymous]
    [HttpGet("active-games")]
    public IActionResult GetActiveGames()
    {
        var activeGames = _activeGameService.GetActiveGames();
        return Ok(activeGames);
    }
}