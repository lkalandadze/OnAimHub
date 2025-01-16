﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Update;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetById;
using OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Create;
using OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Commands.Update;
using OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Create;
using OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.WithdrawOptionFeatures.Commands.Update;
using OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Create;
using OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Delete;
using OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Commands.Update;
using OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Queries.GetById;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.API.Controllers;

public class CoinController : ApiControllerBase
{
    [HttpPost(nameof(CreateWithdrawOption))]
    public async Task<IActionResult> CreateWithdrawOption([FromBody] CreateWithdrawOptionDto create)
        => Ok(await Mediator.Send(new CreateWithdrawOptionCommand(create)));

    [HttpPut(nameof(UpdateWithdrawOption))]
    public async Task<IActionResult> UpdateWithdrawOption([FromBody] UpdateWithdrawOptionDto create)
        => Ok(await Mediator.Send(new UpdateWithdrawOptionCommand(create)));

    [HttpPost(nameof(DeleteWithdrawOption))]
    public async Task<IActionResult> DeleteWithdrawOption([FromQuery] List<int> id)
        => Ok(await Mediator.Send(new DeleteWithdrawOptionCommand(id)));

    [HttpGet(nameof(GetAllWithdrawOptions))]
    public async Task<IActionResult> GetAllWithdrawOptions([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllWithdrawOptionsQuery(filter)));

    [HttpGet(nameof(GetWithdrawOptionById))] 
    public async Task<IActionResult> GetWithdrawOptionById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetWithdrawOptionByIdQuery(id)));

    [HttpPost(nameof(CreateWithdrawOptionGroup))]
    public async Task<IActionResult> CreateWithdrawOptionGroup([FromBody] CreateWithdrawOptionGroupDto create)
        => Ok(await Mediator.Send(new CreateWithdrawOptionGroupCommand(create)));

    [HttpPut(nameof(UpdateWithdrawOptionGroup))]
    public async Task<IActionResult> UpdateWithdrawOptionGroup([FromBody] UpdateWithdrawOptionGroupDto create)
        => Ok(await Mediator.Send(new UpdateWithdrawOptionGroupCommand(create)));

    [HttpPost(nameof(DeleteWithdrawOptiongroup))]
    public async Task<IActionResult> DeleteWithdrawOptiongroup([FromQuery] List<int> id)
        => Ok(await Mediator.Send(new DeleteWithdrawOptiongroupCommand(id)));

    [HttpGet(nameof(GetAllWithdrawOptionGroups))]
    public async Task<IActionResult> GetAllWithdrawOptionGroups([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllWithdrawOptionGroupsQuery(filter)));

    [HttpGet(nameof(GetWithdrawOptionGroupById))]
    public async Task<IActionResult> GetWithdrawOptionGroupById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetWithdrawOptionGroupByIdQuery(id)));

    [HttpPost(nameof(CreateWithdrawOptionEndpoint))]
    public async Task<IActionResult> CreateWithdrawOptionEndpoint([FromBody] CreateWithdrawOptionEndpointDto create)
        => Ok(await Mediator.Send(new CreateWithdrawOptionEndpointCommand(create)));

    [HttpPut(nameof(UpdateWithdrawOptionEndpoint))]
    public async Task<IActionResult> UpdateWithdrawOptionEndpoint([FromBody] UpdateWithdrawOptionEndpointDto create)
        => Ok(await Mediator.Send(new UpdateWithdrawOptionEndpointCommand(create)));

    [HttpPost(nameof(DeleteWithdrawOptionEndpoint))]
    public async Task<IActionResult> DeleteWithdrawOptionEndpoint([FromQuery] List<int> id)
        => Ok(await Mediator.Send(new DeleteWithdrawOptionEndpointCommand(id)));

    [HttpGet(nameof(GetAllWithdrawOptionEndpoints))]
    public async Task<IActionResult> GetAllWithdrawOptionEndpoints([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetWithdrawOptionEndpointsQuery(filter)));

    [HttpGet(nameof(GetWithdrawOptionEndpointById))]
    public async Task<IActionResult> GetWithdrawOptionEndpointById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetWithdrawOptionEndpointByIdQuery(id)));

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
