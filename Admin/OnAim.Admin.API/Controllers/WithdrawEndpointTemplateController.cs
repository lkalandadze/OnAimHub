using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.WithdrawEndpointTemplate;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.API.Controllers;

public class WithdrawEndpointTemplateController : ApiControllerBase
{
    private readonly IWithdrawEndpointTemplateService _withdrawEndpointTemplateService;

    public WithdrawEndpointTemplateController(IWithdrawEndpointTemplateService withdrawEndpointTemplateService)
    {
        _withdrawEndpointTemplateService = withdrawEndpointTemplateService;
    }

    [HttpGet(nameof(GetAllWithdrawEndpointTemplates))]
    public async Task<IActionResult> GetAllWithdrawEndpointTemplates([FromQuery] BaseFilter filter)
    {
        return Ok(await _withdrawEndpointTemplateService.GetAllWithdrawEndpointTemplates(filter));
    }

    [HttpGet(nameof(GetWithdrawEndpointTemplateById))]
    public async Task<IActionResult> GetWithdrawEndpointTemplateById([FromQuery] string CoinTemplateId)
    {
        if (!ObjectId.TryParse(CoinTemplateId, out var objectId))
        {
            return BadRequest("Invalid CoinTemplateId format.");
        }
        return Ok(await _withdrawEndpointTemplateService.GetById(objectId));
    }

    [HttpPost(nameof(CreateWithdrawEndpointTemplate))]
    public async Task<IActionResult> CreateWithdrawEndpointTemplate([FromBody] CreateWithdrawEndpointTemplateDto create)
    {
        await _withdrawEndpointTemplateService.CreateWithdrawEndpointTemplate(create);

        return Ok();
    }

    [HttpPut(nameof(UpdateWithdrawEndpointTemplate))]
    public async Task<IActionResult> UpdateWithdrawEndpointTemplate([FromBody] UpdateWithdrawEndpointTemplateDto update)
    {
        await _withdrawEndpointTemplateService.UpdateWithdrawEndpointTemplate(update);

        return Ok();
    }
}
