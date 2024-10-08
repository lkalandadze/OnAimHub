using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

public class GameController : BaseApiController
{
    private readonly GameSettings _gameSettings;

    public GameController(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
    }

    [HttpGet("Status")]
    public IActionResult GetGameStatus()
    {
        return Ok(new { IsActive = _gameSettings.IsActive.Value });
    }

    [HttpPost("Activate")]
    public IActionResult ActivateGame()
    {
        _gameSettings.SetValue(_gameSettings.IsActive, nameof(_gameSettings.IsActive), true);
        return Ok(new { Message = "Game activated." });
    }

    [HttpPost("Deactivate")]
    public IActionResult DeactivateGame()
    {
        _gameSettings.SetValue(_gameSettings.IsActive, nameof(_gameSettings.IsActive), false);
        return Ok(new { Message = "Game deactivated." });
    }
}