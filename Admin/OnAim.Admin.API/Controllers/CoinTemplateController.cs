using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.CoinFeatures.Queries.GetAllCoin;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.API.Controllers;

public class CoinTemplateController : ApiControllerBase
{
    private readonly ICoinService _coinService;

    public CoinTemplateController(ICoinService coinService)
    {
        _coinService = coinService;
    }

    [HttpGet(nameof(GetAllCoinTemplates))]
    public async Task<IActionResult> GetAllCoinTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllCoinQuery(filter)));

    [HttpGet(nameof(GetCoinTemplateById))]
    public async Task<IActionResult> GetCoinTemplateById([FromQuery] string CoinTemplateId)
    {
        return Ok(await _coinService.GetById(CoinTemplateId));
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
        await _coinService.DeleteCoinTemplate(CoinTemplateId);

        return Ok();
    }
}
