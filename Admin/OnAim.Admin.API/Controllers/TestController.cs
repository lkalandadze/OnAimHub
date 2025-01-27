using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Services.Hub.Promotion;

namespace OnAim.Admin.API.Controllers;

public class TestController : ApiControllerBase
{
    private PromotionService _promotionService;
    public TestController(PromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpPost]
    public async Task<IActionResult> Test()
    {
        var promotionData = new CreatePromotionDto();

        var guid = await _promotionService.CreatePromotion(promotionData);

        return Ok(guid);
    }
}
