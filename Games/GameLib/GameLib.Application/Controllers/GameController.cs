using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

[ApiExplorerSettings(GroupName = "game")]
public class GameController : BaseApiController
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    #region Game

    [AllowAnonymous]
    [HttpGet(nameof(GameVersion))]
    public ActionResult<GameResponseModel> GameVersion()
    {
        var game = _gameService.GetGame();
        _gameService.UpdateMetadataAsync();

        return Ok(game);
    }

    #endregion
}