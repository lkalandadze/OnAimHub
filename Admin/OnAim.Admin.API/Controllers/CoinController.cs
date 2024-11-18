using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.CoinFeatures.Commands.Update;
using OnAim.Admin.APP.Features.CoinFeatures.Queries.GetAllCoin;
using OnAim.Admin.APP.Features.CoinFeatures.Queries.GetById;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.API.Controllers;

public class CoinController : ApiControllerBase
{
    [HttpGet(nameof(GetAllCoins))]
    public async Task<IActionResult> GetAllCoins([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllCoinQuery(filter)));

    [HttpGet(nameof(GetCoinById))]
    public async Task<IActionResult> GetCoinById([FromQuery] GetCoinByIdQuery query)
        => Ok(await Mediator.Send(query));

    [HttpPut(nameof(UpdateCoinForPromotions))]
    public async Task<IActionResult> UpdateCoinForPromotions([FromBody] UpdateCoinForPromotionsCommand command)
        => Ok(await Mediator.Send(command));
}
