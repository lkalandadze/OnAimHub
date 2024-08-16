using Microsoft.AspNetCore.Mvc;
using Wheel.Application;

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
    public async Task<ActionResult<InitialDataResponse>> GetInitialDataAsync()
    {
        var result = _gameManager.GetInitialData();
        return Ok(result);
    }
     
    [HttpPost("play")]
    public async Task<ActionResult<PlayResult>> PlayAsync([FromBody] PlayCommand command)
    {
        var result = _gameManager.Play(command);
        return Ok(result);
    }
}