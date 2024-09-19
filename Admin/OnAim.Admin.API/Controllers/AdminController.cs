﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Commands.Domain.Create;
using OnAim.Admin.APP.Commands.Domain.Delete;
using OnAim.Admin.APP.Queries.Domain.GetAll;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.API.Controllers
{
    public class AdminController : ApiControllerBase
    {
        private readonly IAppSettingsService _appSettingsService;

        public AdminController(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        [HttpGet("Settings")]
        public IActionResult GetSettings()
        {
            var domainRestrictionsEnabled = _appSettingsService.GetSetting("DomainRestrictionsEnabled") != "false";
            return Ok(new { DomainRestrictionsEnabled = domainRestrictionsEnabled });
        }

        [HttpPost("DomainRestrictions")]
        public IActionResult SetDomainRestrictions([FromBody] bool enableRestrictions)
        {
            var settingValue = enableRestrictions ? "true" : "false";
            _appSettingsService.SetSetting("DomainRestrictionsEnabled", settingValue);
            return NoContent();
        }

        [HttpGet("GetAllDomain")]
        public async Task<IActionResult> GetAllDomain()
            => Ok(await Mediator.Send(new GetAllDomainQuery()));

        [HttpPost("InsertDomain")]
        public async Task<IActionResult> InsertDomain([FromBody] CreateEmailDomainCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost("DeleteEmailDomain")]
        public async Task<IActionResult> DeleteEmailDomain([FromBody] DeleteEmailDomainCommand command)
            => Ok(await Mediator.Send(command));
    }
}
