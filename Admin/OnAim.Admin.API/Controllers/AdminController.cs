using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;
using OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.Contracts.Models;
using OnAim.Admin.APP.Services.Admin.SettingServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.API.Controllers;

public class AdminController : ApiControllerBase
{
    private readonly AppSettings _appSettings;

    public AdminController(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    [HttpGet(nameof(GetSettings))]
    public ActionResult<object> GetSettings()
    {
        return Ok(new 
        {
            DomainRestrictionsEnabled = _appSettings.DomainRestrictionsEnabled.Value,
            TwoFactorAuth = _appSettings.TwoFactorAuth.Value,
        });
    }

    [HttpPost(nameof(SetDomainRestrictions))]
    public IActionResult SetDomainRestrictions([FromBody] bool enableRestrictions)
    {
        _appSettings.SetValue(nameof(_appSettings.DomainRestrictionsEnabled), enableRestrictions);
        return NoContent();
    }

    [HttpPost(nameof(SetTwoFactorAuth))]
    public async Task<ActionResult<bool>> SetTwoFactorAuth([FromBody] bool twoFactorAuth)
        => Ok(await _appSettings.SetTwoFactorAuth(twoFactorAuth));

    [HttpGet(nameof(GetAllDomain))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<DomainDto>>>> GetAllDomain([FromQuery] DomainFilter filter)
        => Ok(await Mediator.Send(new GetAllDomainQuery(filter)));

    [HttpPost(nameof(InsertDomain))]
    public async Task<ActionResult<ApplicationResult<bool>>> InsertDomain([FromBody] CreateEmailDomainCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPost(nameof(DeleteEmailDomain))]
    public async Task<ActionResult<ApplicationResult<bool>>> DeleteEmailDomain([FromBody] DeleteEmailDomainCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetActionTypes))]
    [AllowAnonymous]
    public ActionResult<List<string>> GetActionTypes()
    {
        return Ok(ActionTypes.All);
    }

    [HttpGet(nameof(GetEntityNames))]
    [AllowAnonymous]
    public ActionResult<List<string>> GetEntityNames()
    {
        return Ok(EntityNames.All);
    }
}
