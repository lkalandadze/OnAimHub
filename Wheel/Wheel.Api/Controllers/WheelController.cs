using Microsoft.AspNetCore.Mvc;
using Wheel.Application;
using Wheel.Application.Models;

namespace Wheel.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WheelController : ControllerBase
{
    private readonly GameManager _gameManager;

    public WheelController(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    [HttpGet("initial-data")]
    public async Task<ActionResult<InitialDataResponseModel>> GetInitialDataAsync()
    {
        var result = _gameManager.GetInitialData();
        return Ok(result);
    }
     
    [HttpPost("play")]
    public async Task<ActionResult<PlayResultModel>> PlayAsync([FromBody] PlayRequestModel command)
    {
        var result = _gameManager.Play(command);
        return Ok(result);
    }
}