using Microsoft.AspNetCore.Mvc;
using Wheel.Application;
using Wheel.Application.Models;
using Wheel.Application.Models.Player;

namespace Wheel.Api.Controllers;

public class HomeController : BaseApiController
{
    private readonly GameManager _gameManager;

    public HomeController(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    [HttpGet("initial-data")]
    public ActionResult<InitialDataResponseModel> GetInitialDataAsync()
    {
        var result = _gameManager.GetInitialData();
        return Ok(result);
    }
     
    [HttpPost("wheel-play")]
    public async Task<ActionResult<PlayResultModel>> WheelPlayAsync([FromBody] PlayRequestModel command)
    {
        var result = await _gameManager.WheelPlayAsync(command);
        return Ok(result);
    }
}