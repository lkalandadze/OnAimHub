using Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;
using Hub.Application.Features.TransactionFeatures.Commands.CreateWinTransaction;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[ApiExplorerSettings(GroupName = "game")]
public class GameController : BaseApiController
{
    #region Transactions

    [HttpPost(nameof(WinTransaction))]
    public async Task<IActionResult> WinTransaction([FromBody] CreateWinTransactionCommand command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    [HttpPost(nameof(BetTransaction))]
    public async Task<IActionResult> BetTransaction([FromBody] CreateBetTransactionCommand command)
    {
        var result = await Mediator.Send(command);

        return StatusCode(201, result);
    }

    #endregion
}