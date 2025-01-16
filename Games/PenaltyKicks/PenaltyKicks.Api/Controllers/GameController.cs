using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using PenaltyKicks.Application.Models.PenaltyKicks;
using PenaltyKicks.Application.Services.Abstract;
using System.ComponentModel.DataAnnotations;

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
    public ActionResult<InitialDataResponseModel> InitialData([Required] int promotionId)
    {
        return Ok(_penaltyService.GetInitialDataAsync(promotionId));
    }

    [HttpPost(nameof(PlaceBet))]
    public async Task<ActionResult<BetResponseModel>> PlaceBet([Required] int promotionId, [Required] int betPriceId)
    {
        return Ok(await _penaltyService.PlaceBetAsync(promotionId, betPriceId));
    }

    [HttpPost(nameof(PenaltyKick))]
    public async Task<ActionResult<KickResponseModel>> PenaltyKick([Required] int promotionId)
    {
        return Ok(await _penaltyService.PenaltyKick(promotionId));
    }
}