using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using TestWheel.Application.Models.Player;
using TestWheel.Application.Services.Abstract;

namespace TestWheel.Api.Controllers;

[ApiExplorerSettings(GroupName = "game")]
public class GameController : BaseApiController
{
    private readonly ITestWheelService _gameService;

    public GameController(ITestWheelService gameService)
    {
        _gameService = gameService;
    }
     
    [HttpPost(nameof(PlayJackpot))]
    public async Task<ActionResult<PlayResponseModel>> PlayJackpot([FromBody] PlayRequestModel model)
    {
        return Ok(await _gameService.PlayJackpotAsync(model));
    }

    [HttpPost(nameof(PlayWheel))]
    public async Task<ActionResult<PlayResponseModel>> PlayWheel([FromBody] PlayRequestModel model)
    {
        return Ok(await _gameService.PlayWheelAsync(model));
    }
}