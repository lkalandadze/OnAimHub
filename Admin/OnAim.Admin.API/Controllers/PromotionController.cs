using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Create;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Delete;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetById;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetById;
using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Infrasturcture;

namespace OnAim.Admin.API.Controllers;

public class PromotionController : ApiControllerBase
{
    private readonly IPromotionService _promotionService;

    public PromotionController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    //Promotion

    [HttpPost(nameof(CreatePromotion))]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionDto create)
        => Ok(await _promotionService.CreatePromotion(create));

    [HttpGet(nameof(GetAllPromotion))]
    public async Task<IActionResult> GetAllPromotion([FromQuery] PromotionFilter filter)
        => Ok(await Mediator.Send(new GetAllPromotionsQuery(filter)));

    [HttpGet(nameof(GetPromotionById))]
    public async Task<IActionResult> GetPromotionById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetPromotionByIdQuery(id)));

    [HttpGet(nameof(GetAllPromotionGames) + "/{id}")]
    public async Task<IActionResult> GetAllPromotionGames([FromRoute] int id, [FromQuery] BaseFilter filter)
        => Ok(await _promotionService.GetAllPromotionGames(id, filter));

    [HttpGet(nameof(GetPromotionPlayers) + "/{id}")]
    public async Task<IActionResult> GetPromotionPlayers([FromRoute] int id, [FromQuery] PlayerFilter filter)
        => Ok(await _promotionService.GetPromotionPlayers(id, filter));

    [HttpGet(nameof(GetPromotionPlayerTransaction) + "/{id}")]
    public async Task<IActionResult> GetPromotionPlayerTransaction([FromRoute] int id)
        => Ok(await _promotionService.GetPromotionPlayerTransaction(id, new PlayerTransactionFilter()));

    [HttpGet(nameof(GetPromotionLeaderboards) + "/{id}")]
    public async Task<IActionResult> GetPromotionLeaderboards([FromRoute] int id, [FromQuery] BaseFilter filter)
        => Ok(await _promotionService.GetPromotionLeaderboards(id, filter));

    [HttpGet(nameof(GetPromotionLeaderboardDetails) + "/{id}")]
    public async Task<IActionResult> GetPromotionLeaderboardDetails([FromRoute] int id, [FromQuery] BaseFilter filter)
        => Ok(await _promotionService.GetPromotionLeaderboardDetails(id, filter));

    [HttpPost(nameof(CreatePromotionView))]
    public async Task<IActionResult> CreatePromotionView([FromBody] APP.CreatePromotionView create)
        => Ok(await _promotionService.CreatePromotionView(create));

    [HttpPut(nameof(UpdatePromotionStatus))]
    public async Task<IActionResult> UpdatePromotionStatus([FromBody] APP.UpdatePromotionStatusCommand update)
        => Ok(await _promotionService.UpdatePromotionStatus(update));

    [HttpDelete(nameof(DeletePromotion))]
    public async Task<IActionResult> DeletePromotion([FromBody] SoftDeletePromotionCommand command)
        => Ok(await _promotionService.DeletePromotion(command));

    //Promotion Template

    [HttpPost(nameof(CreatePromotionTemplate))]
    public async Task<IActionResult> CreatePromotionTemplate([FromBody] CreatePromotionTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetAllPromotionTemplates))]
    public async Task<IActionResult> GetAllPromotionTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllPromotionTemplatesQuery(filter)));

    [HttpGet(nameof(GetPromotionTemplateById))]
    public async Task<IActionResult> GetPromotionTemplateById([FromQuery] string id)
        => Ok(await Mediator.Send(new GetPromotionTemplateByIdQuery(id)));

    [HttpDelete(nameof(DeletePromotionTemplate))]
    public async Task<IActionResult> DeletePromotionTemplate([FromQuery] DeletePromotionTemplateCommand command)
        => Ok(await Mediator.Send(command));

    //Promotion View Template

    [HttpGet(nameof(GetAllPromotionViewTemplates))]
    public async Task<IActionResult> GetAllPromotionViewTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllPromotionViewTemplatesQuery(filter)));

    [HttpGet(nameof(GetPromotionViewTemplateById))]
    public async Task<IActionResult> GetPromotionViewTemplateById([FromQuery] string id)
        => Ok(await Mediator.Send(new GetPromotionViewTemplateByIdQuery(id)));

    [HttpPost(nameof(CreatePromotionViewTemplate))]
    public async Task<IActionResult> CreatePromotionViewTemplate([FromBody] CreatePromotionViewTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpDelete(nameof(DeletePromotionViewTemplate))]
    public async Task<IActionResult> DeletePromotionViewTemplate([FromQuery] DeletePromotionViewTemplateCommand command)
        => Ok(await Mediator.Send(command));
}