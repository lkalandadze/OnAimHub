using GameLib.Application.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using static Wheel.Application.Services.Concrete.GameService;

namespace Wheel.Api.Controllers;

public class GameController : BaseApiController
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("InitialData")]
    public ActionResult<InitialDataResponseModel> GetInitialDataAsync()
    {
        var result = _gameService.GetInitialData();
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet(nameof(GetGameVersion))]
    public ActionResult<GameResponseModel> GetGameVersion()
    {
        var game = _gameService.GetGame();
        _gameService.UpdateMetadataAsync();
        return Ok(game);
    }
     
    [HttpPost("PlayJackpot")]
    public async Task<ActionResult<PlayResponseModel>> PlayJackpotAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayJackpotAsync(model);
        return Ok(result);
    }

    [HttpPost("PlayWheel")]
    public async Task<ActionResult<PlayResponseModel>> PlayWheelAsync([FromBody] PlayRequestModel model)
    {
        var result = await _gameService.PlayWheelAsync(model);
        return Ok(result);
    }

    [HttpPost("create-configuration-and-round")]
    public async Task<IActionResult> CreateConfigurationAndRoundAsync([FromBody] CreateConfigurationAndRoundRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Call the service method to create configuration and rounds
        var (configModel, roundModel) = await _gameService.CreateConfigurationAndRoundsAsync(
            request.ConfigurationName,
            request.ConfigurationValue,
            request.Rule,
            request.Rounds, // Updated to pass list of rounds
            request.Prices,
            request.Segments);

        // Return the created Configuration and Rounds details
        return Ok(new
        {
            Configuration = configModel,
            Rounds = roundModel // Changed Round to Rounds for consistency
        });
    }

    public class CreateConfigurationAndRoundRequest
    {
        public string ConfigurationName { get; set; }
        public int ConfigurationValue { get; set; }
        public string Rule { get; set; }

        // Expect a list of rounds with just the PrizeId
        public List<RoundModel> Rounds { get; set; }

        public List<PriceModel> Prices { get; set; }
        public List<SegmentModel> Segments { get; set; }
    }
}