using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.CoinFeatures.Queries.GetAllCoin;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.API.Controllers;

public class CoinController : ApiControllerBase
{
    private readonly ICoinTemplateService _coinTemplateService;
    private readonly ICoinService _coinService;

    public CoinController(
        ICoinTemplateService coinTemplateService,
        ICoinService coinService
        )
    {
        _coinTemplateService = coinTemplateService;
        _coinService = coinService;
    }

    [HttpPost(nameof(CreateWithdrawOption))]
    public async Task<IActionResult> CreateWithdrawOption([FromBody] APP.CreateWithdrawOption create)
        => Ok(await _coinService.CreateWithdrawOption(create));

    [HttpPut(nameof(UpdateWithdrawOption))]
    public async Task<IActionResult> UpdateWithdrawOption([FromBody] APP.UpdateWithdrawOption create)
        => Ok(await _coinService.UpdateWithdrawOption(create));

    [HttpPost(nameof(CreateWithdrawOptionGroup))]
    public async Task<IActionResult> CreateWithdrawOptionGroup([FromBody] APP.CreateWithdrawOptionGroup create)
        => Ok(await _coinService.CreateWithdrawOptionGroup(create));

    [HttpPut(nameof(UpdateWithdrawOptionGroup))]
    public async Task<IActionResult> UpdateWithdrawOptionGroup([FromBody] APP.UpdateWithdrawOptionGroup create)
        => Ok(await _coinService.UpdateWithdrawOptionGroup(create));

    [HttpPost(nameof(CreateWithdrawOptionEndpoint))]
    public async Task<IActionResult> CreateWithdrawOptionEndpoint([FromBody] APP.CreateWithdrawOptionEndpoint create)
        => Ok(await _coinService.CreateWithdrawOptionEndpoint(create));

    [HttpPost(nameof(UpdateWithdrawOptionEndpoint))]
    public async Task<IActionResult> UpdateWithdrawOptionEndpoint([FromBody] APP.UpdateWithdrawOptionEndpoint create)
        => Ok(await _coinService.UpdateWithdrawOptionEndpoint(create));

    //Template

    [HttpPost(nameof(CreateCoinTemplate))]
    public async Task<IActionResult> CreateCoinTemplate([FromBody] CreateCoinTemplateDto coinTemplate)
    {
        await _coinTemplateService.CreateCoinTemplate(coinTemplate);
        return Ok();
    }

    [HttpPut(nameof(UpdateCoinTemplate))]
    public async Task<IActionResult> UpdateCoinTemplate([FromBody] UpdateCoinTemplateDto coinTemplate)
    {
        await _coinTemplateService.UpdateCoinTemplate(coinTemplate);
        return Ok();
    }

    [HttpDelete(nameof(DeleteCoinTemplate))]
    public async Task<IActionResult> DeleteCoinTemplate([FromQuery] string CoinTemplateId)
    {
        await _coinTemplateService.DeleteCoinTemplate(CoinTemplateId);

        return Ok();
    }

    [HttpGet(nameof(GetAllCoinTemplates))]
    public async Task<IActionResult> GetAllCoinTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllCoinQuery(filter)));

    [HttpGet(nameof(GetCoinTemplateById))]
    public async Task<IActionResult> GetCoinTemplateById([FromQuery] string CoinTemplateId)
    {
        return Ok(await _coinTemplateService.GetById(CoinTemplateId));
    }
}
