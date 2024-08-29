using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Application.Models;
using Wheel.Infrastructure.Services.Abstract;
using Wheel.Application.Models.Player;

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
    public ActionResult<GameVersionResponseModel> GetGameVersion()
    {
        return Ok(_gameService.GetGame());
    }
     
    [HttpPost("play-jackpot")]
    public async Task<ActionResult<PlayResultModel>> PlayJackpotAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayJackpotAsync(model);
        return Ok(result);
    }

    [HttpPost("play-wheel")]
    public async Task<ActionResult<PlayResultModel>> PlayWheelAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayWheelAsync(model);
        return Ok(result);
    }
}