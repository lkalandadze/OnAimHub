﻿using GameLib.Application.Generators;
using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameLib.Application.Controllers;

//[Authorize(AuthenticationSchemes = "BasicAuthentication")]
[ApiExplorerSettings(GroupName = "admin")]
public class AdminController : BaseApiController
{
    private readonly IGameService _gameService;
    private readonly IGameConfigurationService _configurationService;
    //private readonly IPrizeTypeService _prizeTypeService;

    public AdminController(IGameService gameService, IGameConfigurationService configurationService/*, IPrizeTypeService prizeTypeService*/)
    {
        _gameService = gameService;
        _configurationService = configurationService;
        //_prizeTypeService = prizeTypeService;
    }

    #region Game

    [HttpPost(nameof(ResetInMemoryData))]
    public ActionResult ResetInMemoryData()
    {
        _configurationService.ResetInMemoryData();

        return Ok(new { Message = "In memory data have been reset." });
    }

    [HttpGet(nameof(GameStatus))]
    public ActionResult GameStatus()
    {
        return Ok(_gameService.GameStatus());
    }

    [HttpPost(nameof(ActivateGame))]
    public ActionResult ActivateGame()
    {
        _gameService.ActivateGame();

        return Ok(new { Message = "Game activated." });
    }

    [HttpPost(nameof(DeactivateGame))]
    public ActionResult DeactivateGame()
    {
        _gameService.DeactivateGame();

        return Ok(new { Message = "Game deactivated." });
    }

    #endregion

    #region Configurations

    [HttpGet(nameof(ConfigurationMetadata))]
    public ActionResult<EntityMetadata> ConfigurationMetadata()
    {
        return Ok(_configurationService.GetConfigurationMetaData());
    }

    [HttpGet(nameof(Configurations))]
    public async Task<ActionResult<IEnumerable<ConfigurationBaseGetModel>>> Configurations(int? configurationId, int? promotionId)
    {
        return Ok(await _configurationService.GetAllAsync(configurationId, promotionId));
    }

    //[HttpGet(nameof(ConfigurationById))]
    //public async Task<ActionResult<GameConfiguration>> ConfigurationById(int id)
    //{
    //    return Ok(await _configurationService.GetByIdAsync(id));
    //}

    [HttpPost(nameof(CreateConfiguration))]
    public async Task<ActionResult> CreateConfiguration(GameConfiguration configuration)
    {
        await _configurationService.CreateAsync(configuration);
        return StatusCode(200);
    }

    [HttpPut(nameof(UpdateConfiguration))]
    public async Task<ActionResult> UpdateConfiguration(GameConfiguration configuration)
    {
        await _configurationService.UpdateAsync(configuration);
        return StatusCode(200);
    }

    [HttpPatch(nameof(ActivateConfiguration))]
    public async Task<ActionResult> ActivateConfiguration(int id)
    {
        await _configurationService.ActivateAsync(id);
        return StatusCode(200);
    }

    [HttpPatch(nameof(DeactivateConfiguration))]
    public async Task<ActionResult> DeactivateConfiguration(int id)
    {
        await _configurationService.DeactivateAsync(id);
        return StatusCode(200);
    }

    [HttpDelete(nameof(DeleteConfiguration))]
    public async Task<ActionResult> DeleteConfiguration([Required] int id)
    {
        await _configurationService.DeleteAsync(id);
        return StatusCode(200);
    }

    [HttpDelete(nameof(DeleteByCorrelationId))]
    public async Task<ActionResult> DeleteByCorrelationId([Required] Guid id)
    {
        await _configurationService.DeleteByCorrelationIdAsync(id);
        return StatusCode(200);
    }

    #endregion

    #region PrizeType

    //[HttpGet(nameof(PrizeTypes))]
    //public async Task<ActionResult<IEnumerable<PrizeTypeGetModel>>> PrizeTypes()
    //{
    //    return Ok(await _prizeTypeService.GetAllAsync());
    //}

    //[HttpGet(nameof(PrizeTypeById))]
    //public async Task<ActionResult<PrizeTypeGetModel>> PrizeTypeById(int id)
    //{
    //    return Ok(await _prizeTypeService.GetByIdAsync(id));
    //}

    //[HttpPost(nameof(CreatePrizeType))]
    //public async Task<ActionResult> CreatePrizeType([FromBody] PrizeTypeCreateModel model)
    //{
    //    await _prizeTypeService.CreateAsync(model);
    //    return StatusCode(201);
    //}

    //[HttpPut(nameof(UpdatePrizeType))]
    //public async Task<ActionResult> UpdatePrizeType([FromRoute] int id, [FromBody] PrizeTypeUpdateModel model)
    //{
    //    await _prizeTypeService.UpdateAsync(id, model);
    //    return StatusCode(200);
    //}

    #endregion
}