using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.Contracts.Dtos;
using OnAim.Admin.Contracts.Models;

namespace OnAim.Admin.API.Controllers;

public class AdminController : ApiControllerBase
{
    private readonly IAppSettingsService _appSettingsService;

    public AdminController(IAppSettingsService appSettingsService)
    {
        _appSettingsService = appSettingsService;
    }

    [HttpGet(nameof(GetSettings))]
    public IActionResult GetSettings()
    {
        var domainRestrictionsEnabled = _appSettingsService.GetSettings();
        return Ok(new { DomainRestrictionsEnabled = domainRestrictionsEnabled });
    }

    [HttpPost(nameof(SetDomainRestrictions))]
    public IActionResult SetDomainRestrictions([FromBody] bool enableRestrictions)
    {
        var settingValue = enableRestrictions ? "true" : "false";
        _appSettingsService.SetSetting("DomainRestrictionsEnabled", settingValue);
        return NoContent();
    }

    [HttpPost(nameof(SetTwoFactorAuth))]
    public async Task<IActionResult> SetTwoFactorAuth([FromBody] bool twoFactorAuth)
        => Ok(await _appSettingsService.SetTwoFactorAuth(twoFactorAuth));

    [HttpGet(nameof(GetAllDomain))]
    public async Task<IActionResult> GetAllDomain([FromQuery] DomainFilter filter)
        => Ok(await Mediator.Send(new GetAllDomainQuery(filter)));

    [HttpPost(nameof(InsertDomain))]
    public async Task<IActionResult> InsertDomain([FromBody] CreateEmailDomainCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPost(nameof(DeleteEmailDomain))]
    public async Task<IActionResult> DeleteEmailDomain([FromBody] DeleteEmailDomainCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetActionTypes))]
    [AllowAnonymous]
    public IActionResult GetActionTypes()
    {
        return Ok(ActionTypes.All);
    }

    [HttpGet(nameof(GetEntityNames))]
    [AllowAnonymous]
    public IActionResult GetEntityNames()
    {
        return Ok(EntityNames.All);
    }
}
