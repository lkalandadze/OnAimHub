using Hub.Application.Features.GameFeatures.Queries.GetActiveGames;
using Hub.Application.Services.Abstract;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEvents.IntegrationEvents;

namespace Hub.Api.Controllers;

public class TestController : BaseApiController
{
    private readonly IBus _bus;
    private readonly IGameService _activeGameService;
    public TestController(IBus bus, IGameService gameService)
    {
        _bus = bus;
        _activeGameService = gameService;
    }

    [HttpPost("spin-wheel")]
    public async Task<IActionResult> SpinWheel(int userId)
    {
        var correlationId = Guid.NewGuid();
        var spinEvent = new SpinRequestedIntegrationEvent(correlationId, userId);

        await _bus.Publish(spinEvent);

        return Ok(new { Message = "Spin request sent successfully!", CorrelationId = correlationId });
    }

    [AllowAnonymous]
    [HttpGet("active-games-anonymous")]
    public IActionResult GetActiveGamesAnonymous()
    {
        var activeGames = _activeGameService.GetGames();
        return Ok(activeGames);
    }

    [HttpGet(nameof(GetActiveGames))]
    public async Task<List<Shared.Lib.Wrappers.Response<GetActiveGamesQueryResponse>>> GetActiveGames([FromQuery] GetActiveGamesQuery request) => await Mediator.Send(request);
}