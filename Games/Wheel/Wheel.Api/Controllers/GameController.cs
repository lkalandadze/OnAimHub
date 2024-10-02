using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using static Wheel.Application.Services.Concrete.GameService;

namespace Wheel.Api.Controllers;

public class GameController : BaseApiController
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("InitialData")]
    public ActionResult<InitialDataResponseModel> GetInitialDataAsync()
    {
        var result = _gameService.GetInitialData();
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet(nameof(GetGameVersion))]
    public ActionResult<GameResponseModel> GetGameVersion()
    {
        var game = _gameService.GetGame();
        _gameService.UpdateMetadataAsync();
        return Ok(game);
    }
     
    [HttpPost("PlayJackpot")]
    public async Task<ActionResult<PlayResponseModel>> PlayJackpotAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayJackpotAsync(model);
        return Ok(result);
    }

    [HttpPost("PlayWheel")]
    public async Task<ActionResult<PlayResponseModel>> PlayWheelAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayWheelAsync(model);
        return Ok(result);
    }
}