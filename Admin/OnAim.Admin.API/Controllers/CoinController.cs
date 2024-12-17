using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Update;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetById;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.Dtos.Base;

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

    [HttpPost(nameof(DeleteWithdrawOption))]
    public async Task<IActionResult> DeleteWithdrawOption([FromBody] APP.DeleteWithdrawOption delete)
        => Ok(await _coinService.DeleteWithdrawOption(delete));

    [HttpGet(nameof(GetAllWithdrawOptions))]
    public async Task<IActionResult> GetAllWithdrawOptions([FromQuery] BaseFilter filter)
        => Ok(await _coinService.GetAllWithdrawOptions(filter));

    [HttpGet(nameof(GetWithdrawOptionById))] 
    public async Task<IActionResult> GetWithdrawOptionById([FromQuery] int id)
        => Ok(await _coinService.GetWithdrawOptionById(id));

    [HttpPost(nameof(CreateWithdrawOptionGroup))]
    public async Task<IActionResult> CreateWithdrawOptionGroup([FromBody] APP.CreateWithdrawOptionGroup create)
        => Ok(await _coinService.CreateWithdrawOptionGroup(create));

    [HttpPut(nameof(UpdateWithdrawOptionGroup))]
    public async Task<IActionResult> UpdateWithdrawOptionGroup([FromBody] APP.UpdateWithdrawOptionGroup create)
        => Ok(await _coinService.UpdateWithdrawOptionGroup(create));

    [HttpPost(nameof(DeleteWithdrawOptiongroup))]
    public async Task<IActionResult> DeleteWithdrawOptiongroup([FromBody] APP.DeleteWithdrawOptionGroup delete)
        => Ok(await _coinService.DeleteWithdrawOptiongroup(delete));

    [HttpGet(nameof(GetAllWithdrawOptionGroups))]
    public async Task<IActionResult> GetAllWithdrawOptionGroups([FromQuery] BaseFilter filter)
        => Ok(await _coinService.GetAllWithdrawOptionGroups(filter));

    [HttpGet(nameof(GetWithdrawOptionGroupById))]
    public async Task<IActionResult> GetWithdrawOptionGroupById([FromQuery] int id)
        => Ok(await _coinService.GetWithdrawOptionGroupById(id));

    [HttpPost(nameof(CreateWithdrawOptionEndpoint))]
    public async Task<IActionResult> CreateWithdrawOptionEndpoint([FromBody] APP.CreateWithdrawOptionEndpoint create)
        => Ok(await _coinService.CreateWithdrawOptionEndpoint(create));

    [HttpPut(nameof(UpdateWithdrawOptionEndpoint))]
    public async Task<IActionResult> UpdateWithdrawOptionEndpoint([FromBody] APP.UpdateWithdrawOptionEndpoint create)
        => Ok(await _coinService.UpdateWithdrawOptionEndpoint(create));

    [HttpPost(nameof(DeleteWithdrawOptionEndpoint))]
    public async Task<IActionResult> DeleteWithdrawOptionEndpoint([FromBody] APP.DeleteWithdrawOptionEndpoint delete)
        => Ok(await _coinService.DeleteWithdrawOptionEndpoint(delete));

    [HttpGet(nameof(GetAllWithdrawOptionEndpoints))]
    public async Task<IActionResult> GetAllWithdrawOptionEndpoints([FromQuery] BaseFilter filter)
        => Ok(await _coinService.GetWithdrawOptionEndpoints(filter));

    [HttpGet(nameof(GetWithdrawOptionEndpointById))]
    public async Task<IActionResult> GetWithdrawOptionEndpointById([FromQuery] int id)
        => Ok(await _coinService.GetWithdrawOptionEndpointById(id));

    //Template

    [HttpPost(nameof(CreateCoinTemplate))]
    public async Task<IActionResult> CreateCoinTemplate([FromBody] CreateCoinTemplateCommand coinTemplate)
        => Ok(await Mediator.Send(coinTemplate));

    [HttpPut(nameof(UpdateCoinTemplate))]
    public async Task<IActionResult> UpdateCoinTemplate([FromBody] UpdateCoinTemplateCommand coinTemplate)
        => Ok(await Mediator.Send(coinTemplate));

    [HttpDelete(nameof(DeleteCoinTemplate))]
    public async Task<IActionResult> DeleteCoinTemplate([FromQuery] DeleteCoinTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetAllCoinTemplates))]
    public async Task<IActionResult> GetAllCoinTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllCoinTemplatesQuery(filter)));

    [HttpGet(nameof(GetCoinTemplateById))]
    public async Task<IActionResult> GetCoinTemplateById([FromQuery] string id)
        => Ok(await Mediator.Send(new GetCoinTemplateByIdQuery(id)));
}
