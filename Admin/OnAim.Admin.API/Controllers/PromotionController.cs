using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.PromotionFeatures.Commands.Create;
using OnAim.Admin.APP.Features.PromotionFeatures.Commands.Update;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetById;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.API.Controllers;

public class PromotionController : ApiControllerBase
{
    private readonly IPromotionService _promotionService;

    public PromotionController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
    [HttpGet(nameof(GetAllPromotion))]
    public async Task<IActionResult> GetAllPromotion([FromQuery] PromotionFilter filter)
        => Ok(await Mediator.Send(new GetAllPromotionsQuery(filter)));

    [HttpGet(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetPromotionByIdQuery(id)));

    [HttpPost(nameof(CreatePromotion))]
    public async Task<IActionResult> CreatePromotion([FromBody] APP.CreatePromotionCommand create)
        => Ok(await _promotionService.CreatePromotion(create));

    [HttpPut(nameof(UpdatePromotion))]
    public async Task<IActionResult> UpdatePromotion([FromBody] UpdatePromotionDto updateCoinInProm)
        => Ok(await Mediator.Send(new UpdatePromotionCommand(updateCoinInProm)));
}
