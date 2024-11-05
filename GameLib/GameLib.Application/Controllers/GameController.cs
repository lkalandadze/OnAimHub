using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

public class GameController : BaseApiController
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    #region Game

    [HttpGet(nameof(InitialData))]
    public ActionResult<GetInitialDataResponseModel> InitialData()
    {
        return Ok(_gameService.GetInitialData());
    }

    #endregion
}