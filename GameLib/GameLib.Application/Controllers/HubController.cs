using GameLib.Application.Models.Configuration;
using GameLib.Application.Models.Game;
using GameLib.Application.Models.PrizeType;
using GameLib.Application.Models.Segment;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

public class HubController : BaseApiController
{
    private readonly IGameService _gameService;
    private readonly IGameConfigurationService _configurationService;
    private readonly ISegmentService _segmentService;
    private readonly IPrizeTypeService _prizeTypeService;

    public HubController(IGameService gameService, IGameConfigurationService configurationService, ISegmentService segmentService, IPrizeTypeService prizeTypeService)
    {
        _gameService = gameService;
        _configurationService = configurationService;
        _segmentService = segmentService;
        _prizeTypeService = prizeTypeService;
    }

    #region Game

    [HttpGet(nameof(GameShortInfo))]
    public async Task<ActionResult<IEnumerable<GetGameShortInfoModel>>> GameShortInfo()
    {
        return Ok(await _gameService.GetGameShortInfo());
    }

    [HttpGet(nameof(GameStatus))]
    public IActionResult GameStatus()
    {
        return Ok(_gameService.GameStatus());
    }

    [HttpPost(nameof(ActivateGame))]
    public IActionResult ActivateGame()
    {
        _gameService.ActivateGame();

        return Ok(new { Message = "Game activated." });
    }

    [HttpPost(nameof(DeactivateGame))]
    public IActionResult DeactivateGame()
    {
        _gameService.DeactivateGame();

        return Ok(new { Message = "Game deactivated." });
    }

    #endregion

    #region Configurations

    [HttpGet(nameof(Configurations))]
    public async Task<ActionResult<IEnumerable<ConfigurationBaseGetModel>>> Configurations()
    {
        return Ok(await _configurationService.GetAllAsync());
    }

    [HttpGet(nameof(ConfigurationById))]
    public async Task<ActionResult<GameConfiguration>> ConfigurationById(int id)
    {
        return Ok(await _configurationService.GetByIdAsync(id));
    }

    [HttpPost(nameof(CreateConfiguration))]
    public async Task<ActionResult> CreateConfiguration([FromBody] ConfigurationCreateModel model)
    {
        await _configurationService.CreateAsync(model);
        return StatusCode(201);
    }

    [HttpPut(nameof(UpdateConfiguration))]
    public async Task<ActionResult> UpdateConfiguration([FromRoute] int id, [FromBody] ConfigurationUpdateModel model)
    {
        await _configurationService.UpdateAsync(id, model);
        return StatusCode(200);
    }

    [HttpPatch(nameof(ActivateConfiguration))]
    public async Task<ActionResult> ActivateConfiguration([FromRoute] int id)
    {
        await _configurationService.ActivateAsync(id);
        return StatusCode(200);
    }

    [HttpPatch(nameof(DeactivateConfiguration))]
    public async Task<ActionResult> DeactivateConfiguration([FromRoute] int id)
    {
        await _configurationService.DeactivateAsync(id);
        return StatusCode(200);
    }

    [HttpPost(nameof(AssignConfigurationToSegments))]
    public async Task<ActionResult> AssignConfigurationToSegments([FromRoute] int id, [FromBody] IEnumerable<string> segmentIds)
    {
        await _configurationService.AssignConfigurationToSegmentsAsync(id, segmentIds);
        return StatusCode(200);
    }

    [HttpPost(nameof(UnassignConfigurationToSegments))]
    public async Task<ActionResult> UnassignConfigurationToSegments([FromRoute] int id, [FromBody] IEnumerable<string> segmentIds)
    {
        await _configurationService.UnassignConfigurationToSegmentsAsync(id, segmentIds);
        return StatusCode(200);
    }

    #endregion

    #region Segments

    [HttpGet(nameof(Segments))]
    public async Task<ActionResult<IEnumerable<SegmentBaseGetModel>>> Segments()
    {
        return Ok(await _segmentService.GetAllAsync());
    }

    [HttpGet(nameof(SegmentById))]
    public async Task<ActionResult<ConfigurationBaseGetModel>> SegmentById(int id)
    {
        return Ok(await _segmentService.GetByIdAsync(id));
    }

    [HttpPost(nameof(CreateSegment))]
    public async Task<ActionResult> CreateSegment([FromBody] SegmentCreateModel model)
    {
        await _segmentService.CreateAsync(model);
        return StatusCode(201);
    }

    [HttpPatch(nameof(DeleteSegment))]
    public async Task<ActionResult> DeleteSegment([FromRoute] int id)
    {
        await _segmentService.DeleteAsync(id);
        return StatusCode(201);
    }

    #endregion

    #region PrizeType

    [HttpGet(nameof(PrizeTypes))]
    public async Task<ActionResult<IEnumerable<PrizeTypeGetModel>>> PrizeTypes()
    {
        return Ok(await _prizeTypeService.GetAllAsync());
    }

    [HttpGet(nameof(PrizeTypeById))]
    public async Task<ActionResult<PrizeTypeGetModel>> PrizeTypeById(int id)
    {
        return Ok(await _prizeTypeService.GetByIdAsync(id));
    }

    [HttpPost(nameof(CreatePrizeType))]
    public async Task<ActionResult> CreatePrizeType([FromBody] PrizeTypeCreateModel model)
    {
        await _prizeTypeService.CreateAsync(model);
        return StatusCode(201);
    }

    [HttpPut(nameof(UpdatePrizeType))]
    public async Task<ActionResult> UpdatePrizeType([FromRoute] int id, [FromBody] PrizeTypeUpdateModel model)
    {
        await _prizeTypeService.UpdateAsync(id, model);
        return StatusCode(200);
    }

    #endregion
}