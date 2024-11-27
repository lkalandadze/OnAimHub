using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.CoinFeatures.Queries.GetAllCoin;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.API.Controllers;

public class CoinController : ApiControllerBase
{
    private readonly ICoinService _coinService;

    public CoinController(ICoinService coinService)
    {
        _coinService = coinService;
    }

    [HttpGet(nameof(GetAllCoinTemplates))]
    public async Task<IActionResult> GetAllCoinTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllCoinQuery(filter)));

    [HttpGet(nameof(GetCoinTemplateById))]
    public async Task<IActionResult> GetCoinTemplateById([FromQuery] string CoinTemplateId)
    {
        if (!ObjectId.TryParse(CoinTemplateId, out var objectId))
        {
            return BadRequest("Invalid CoinTemplateId format.");
        }
        return Ok(await _coinService.GetById(objectId));
    }

    [HttpPost(nameof(CreateCoinTemplate))]
    public async Task<IActionResult> CreateCoinTemplate([FromBody] CreateCoinTemplateDto coinTemplate)
    {
        await _coinService.CreateCoinTemplate(coinTemplate);
        return Ok();
    }

    [HttpPut(nameof(UpdateCoinTemplate))]
    public async Task<IActionResult> UpdateCoinTemplate([FromBody] UpdateCoinTemplateDto coinTemplate)
    {
        await _coinService.UpdateCoinTemplate(coinTemplate);
        return Ok();
    }

    [HttpDelete(nameof(DeleteCoinTemplate))]
    public async Task<IActionResult> DeleteCoinTemplate([FromQuery] string CoinTemplateId)
    {
        if (!ObjectId.TryParse(CoinTemplateId, out var objectId))
        {
            return BadRequest("Invalid CoinTemplateId format.");
        }

        await _coinService.DeleteCoinTemplate(objectId);

        return Ok();
    }
}
