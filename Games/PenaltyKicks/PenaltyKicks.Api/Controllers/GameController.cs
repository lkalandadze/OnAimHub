using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using PenaltyKicks.Application.Models.PenaltyKicks;
using PenaltyKicks.Application.Services.Abstract;

namespace PenaltyKicks.Api.Controllers;

[ApiExplorerSettings(GroupName = "game")]
public class GameController : BaseApiController
{
    private readonly IPenaltyService _penaltyService;

    public GameController(IPenaltyService penaltyService)
    {
        _penaltyService = penaltyService;
    }

    [HttpGet(nameof(InitialData))]
    public async Task<ActionResult<InitialDataResponseModel>> InitialData(int promotionId)
    {
        return Ok(await _penaltyService.GetInitialDataAsync(promotionId));
    }

    [HttpPost(nameof(PlaceBet))]
    public async Task<ActionResult> PlaceBet()
    {
        return Ok();
    }

    [HttpPost(nameof(PenaltyKick))]
    public async Task<ActionResult> PenaltyKick()
    {
        return Ok();
    }
}