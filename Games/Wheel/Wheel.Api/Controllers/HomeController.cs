using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;

namespace Wheel.Api.Controllers;

public class HomeController : BaseApiController
{
    private readonly IGameService _gameService;

    public HomeController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("initial-data")]
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
     
    [HttpPost("play-jackpot")]
    public async Task<ActionResult<PlayResponseModel>> PlayJackpotAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayJackpotAsync(model);
        return Ok(result);
    }

    [HttpPost("play-wheel")]
    public async Task<ActionResult<PlayResponseModel>> PlayWheelAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayWheelAsync(model);
        return Ok(result);
    }
}