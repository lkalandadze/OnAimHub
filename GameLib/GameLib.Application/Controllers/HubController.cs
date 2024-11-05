using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiExplorerSettings(GroupName = "hub")]
public class HubController : BaseApiController
{
    private readonly IGameService _gameService;

    public HubController(IGameService gameService)
    {
        _gameService = gameService;
    }

    #region Game

    [HttpGet(nameof(GameShortInfo))]
    public async Task<ActionResult<IEnumerable<GameShortInfoResponseModel>>> GameShortInfo()
    {
        return Ok(await _gameService.GetGameShortInfo());
    }

    #endregion
}