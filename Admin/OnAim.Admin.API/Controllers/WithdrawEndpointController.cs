using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.WithdrawEndpointTemplate;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.API.Controllers;

public class WithdrawEndpointController : ApiControllerBase
{
    private readonly IWithdrawEndpointTemplateService _withdrawEndpointTemplateService;

    public WithdrawEndpointController(IWithdrawEndpointTemplateService withdrawEndpointTemplateService)
    {
        _withdrawEndpointTemplateService = withdrawEndpointTemplateService;
    }

    [HttpGet(nameof(GetAllWithdrawEndpoints))]
    public async Task<IActionResult> GetAllWithdrawEndpoints([FromQuery] BaseFilter filter)
    {
        return Ok(await _withdrawEndpointTemplateService.GetAllWithdrawEndpointTemplates(filter));
    }

    [HttpGet(nameof(GetWithdrawEndpointById))]
    public async Task<IActionResult> GetWithdrawEndpointById([FromQuery] string CoinTemplateId)
    {
        if (!ObjectId.TryParse(CoinTemplateId, out var objectId))
        {
            return BadRequest("Invalid CoinTemplateId format.");
        }
        return Ok(await _withdrawEndpointTemplateService.GetById(objectId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWithdrawEndpointTemplateDto create)
    {
        await _withdrawEndpointTemplateService.CreateWithdrawEndpointTemplate(create);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWithdrawEndpointTemplateDto update)
    {
        await _withdrawEndpointTemplateService.UpdateWithdrawEndpointTemplate(update);

        return Ok();
    }
}
