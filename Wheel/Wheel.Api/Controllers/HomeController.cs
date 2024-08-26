using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Application.Models;
using Wheel.Infrastructure.Services.Abstract;

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
     
    [HttpPost("play")]
    public ActionResult<PlayResultModel> PlayAsync([FromBody] PlayRequestModel command)
    {
        var result = _gameService.Play(command);
        return Ok(result);
    }
}