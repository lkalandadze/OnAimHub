using Microsoft.AspNetCore.Mvc;
using Wheel.Application;
using Wheel.Application.Models;

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
     
    [HttpPost("play")]
    public ActionResult<PlayResultModel> PlayAsync([FromBody] PlayRequestModel command)
    {
        var result = _gameManager.Play(command);
        return Ok(result);
    }
}