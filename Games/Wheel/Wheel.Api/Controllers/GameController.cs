using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;

namespace Wheel.Api.Controllers;

public class GameController : BaseApiController
{
    private readonly IWheelService _gameService;

    public GameController(IWheelService gameService)
    {
        _gameService = gameService;
    }
     
    [HttpPost(nameof(PlayJackpot))]
    public async Task<ActionResult<PlayResponseModel>> PlayJackpot([FromBody] PlayRequestModel model)
    {
        return Ok(await _gameService.PlayJackpotAsync(model));
    }

    [HttpPost(nameof(PlayWheel))]
    public async Task<ActionResult<PlayResponseModel>> PlayWheel([FromBody] PlayRequestModel model)
    {
        return Ok(await _gameService.PlayWheelAsync(model));
    }
}