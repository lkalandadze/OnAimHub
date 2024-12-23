using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
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
    public ActionResult<InitialDataResponseModel> InitialData(int promotionId)
    {
        return Ok(_gameService.GetInitialData(promotionId));
    }

    [HttpPost(nameof(PlayWheel))]
    public async Task<ActionResult<PlayResponseModel>> PlayWheel([FromBody] PlayRequestModel model)
    {
        return Ok(await _gameService.PlayWheelAsync(model));
    }
}