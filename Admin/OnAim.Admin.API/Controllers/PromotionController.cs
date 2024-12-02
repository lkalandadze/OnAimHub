using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetById;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture;

namespace OnAim.Admin.API.Controllers;

public class PromotionController : ApiControllerBase
{
    private readonly IPromotionService _promotionService;
    private readonly IPromotionViewTemplateService _promotionViewTemplateService;
    private readonly IPromotionTemplateService _promotionTemplateService;

    public PromotionController(
        IPromotionService promotionService, 
        IPromotionViewTemplateService promotionViewTemplateService,
        IPromotionTemplateService promotionTemplateService
        )
    {
        _promotionService = promotionService;
        _promotionViewTemplateService = promotionViewTemplateService;
        _promotionTemplateService = promotionTemplateService;
    }

    [HttpGet(nameof(GetAllPromotion))]
    public async Task<IActionResult> GetAllPromotion([FromQuery] PromotionFilter filter)
        => Ok(await Mediator.Send(new GetAllPromotionsQuery(filter)));

    [HttpGet(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetPromotionByIdQuery(id)));

    [HttpPost(nameof(CreatePromotion))]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionDto create)
        => Ok(await _promotionService.CreatePromotion(create));

    [HttpPost(nameof(CreatePromotionView))]
    public async Task<IActionResult> CreatePromotionView([FromBody] APP.CreatePromotionView create)
        => Ok(await _promotionService.CreatePromotionView(create));

    [HttpPut(nameof(UpdatePromotionStatus))]
    public async Task<IActionResult> UpdatePromotionStatus([FromBody] APP.UpdatePromotionStatusCommand update)
    {
        await _promotionService.UpdatePromotionStatus(update);
        return Ok();
    }

    [HttpPut(nameof(DeletePromotion))]
    public async Task<IActionResult> DeletePromotion([FromBody] SoftDeletePromotionCommand command)
    {
        await _promotionService.DeletePromotion(command);
        return Ok();
    }

    //Template

    [HttpPost]
    public async Task<IActionResult> CreatePromotionTemplate([FromBody] PromotionTemplate promotionTemplate)
    {
        return Ok(await _promotionTemplateService.CreatePromotionTemplate(promotionTemplate));
    }

    [HttpGet(nameof(GetAllPromotionViewTemplates))]
    public async Task<IActionResult> GetAllPromotionViewTemplates()
    {
        return Ok(await _promotionViewTemplateService.GetAllWithdrawEndpointTemplates());
    }

    [HttpGet(nameof(GetPromotionViewTemplateById))]
    public async Task<IActionResult> GetPromotionViewTemplateById([FromQuery] string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid Id format.");
        }
        return Ok(await _promotionViewTemplateService.GetById(objectId));
    }

    [HttpPost(nameof(CreatePromotionViewTemplate))]
    public async Task<IActionResult> CreatePromotionViewTemplate([FromBody] CreatePromotionViewTemplateAsyncDto create)
    {
        await _promotionViewTemplateService.CreatePromotionViewTemplateAsync(create);

        return Ok();
    }
}