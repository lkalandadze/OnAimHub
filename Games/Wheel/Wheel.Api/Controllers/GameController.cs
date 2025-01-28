using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wheel.Application.Models.Wheel;
using Wheel.Application.Services.Abstract;

namespace Wheel.Api.Controllers;

[ApiExplorerSettings(GroupName = "game")]
public class GameController : BaseApiController
{
    private readonly IWheelService _gameService;

    public GameController(IWheelService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet(nameof(InitialData))]
    public ActionResult<InitialDataResponseModel> InitialData([Required] int promotionId)
    {
        return Ok(_gameService.GetInitialData(promotionId));
    }

    [HttpPost(nameof(PlayWheel))]
    public async Task<ActionResult<PlayResponseModel>> PlayWheel([Required] int promotionId, [Required] int betPriceId)
    {
        return Ok(await _gameService.PlayWheelAsync(promotionId, betPriceId));
    }
}