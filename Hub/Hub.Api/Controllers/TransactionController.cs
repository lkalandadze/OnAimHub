using Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;
using Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

public class TransactionController : BaseApiController
{
    [HttpPost("win")]
    public async Task<IActionResult> CreateWinTransactionAsync([FromBody] CreateWinTransaction command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPost("bet")]
    public async Task<IActionResult> CreateBetTransactionAsync([FromBody] CreateBetTransaction command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }
}