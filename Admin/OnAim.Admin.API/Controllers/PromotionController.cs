﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.APP.Services.HubServices.Promotion;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Queries.GetById;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Create;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Commands.Delete;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetAll;
using OnAim.Admin.APP.Features.PromotionFeatures.Template.PromotionView.Queries.GetById;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using System.Text.Json;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.API.Controllers
{
    public class PromotionController : ApiControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly string _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "EventDescriptions");

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        #region Promotion
        [HttpPost(nameof(CreatePromotion))]
        public async Task<ActionResult<ApplicationResult<Guid>>> CreatePromotion([FromBody] CreatePromotionDto create)
            => Ok(await _promotionService.CreatePromotion(create));

        [HttpGet(nameof(GetAllPromotion))]
        public async Task<ActionResult<ApplicationResult<PaginatedResult<PromotionDto>>>> GetAllPromotion([FromQuery] PromotionFilter filter)
            => Ok(await Mediator.Send(new GetAllPromotionsQuery(filter)));

        [HttpGet(nameof(GetPromotionById))]
        public async Task<ActionResult<ApplicationResult<PromotionDto>>> GetPromotionById([FromQuery] int id)
            => Ok(await Mediator.Send(new GetPromotionByIdQuery(id)));

        [HttpGet(nameof(GetAllPromotionGames) + "/{id}")]
        public async Task<ActionResult<object>> GetAllPromotionGames([FromRoute] int id, [FromQuery] BaseFilter filter)
            => Ok(await _promotionService.GetAllPromotionGames(id, filter));

        [HttpGet(nameof(GetPromotionPlayers) + "/{id}")]
        public async Task<ActionResult<ApplicationResult<PaginatedResult<PlayerListDto>>>> GetPromotionPlayers([FromRoute] int id, [FromQuery] PlayerFilter filter)
            => Ok(await _promotionService.GetPromotionPlayers(id, filter));

        [HttpGet(nameof(GetPromotionPlayerTransaction) + "/{id}")]
        public async Task<ActionResult<ApplicationResult<PaginatedResult<PlayerTransactionDto>>>> GetPromotionPlayerTransaction([FromRoute] int id)
            => Ok(await _promotionService.GetPromotionPlayerTransaction(id, new PlayerTransactionFilter()));

        [HttpGet(nameof(GetPromotionLeaderboards) + "/{id}")]
        public async Task<ActionResult<ApplicationResult<PromotionLeaderboardDto<object>>>> GetPromotionLeaderboards([FromRoute] int id, [FromQuery] BaseFilter filter)
            => Ok(await _promotionService.GetPromotionLeaderboards(id, filter));

        [HttpGet(nameof(GetPromotionLeaderboardDetails) + "/{id}")]
        public async Task<ActionResult<ApplicationResult<PaginatedResult<PromotionLeaderboardDetailDto>>>> GetPromotionLeaderboardDetails([FromRoute] int id, [FromQuery] BaseFilter filter)
            => Ok(await _promotionService.GetPromotionLeaderboardDetails(id, filter));

        [HttpPost(nameof(CreatePromotionView))]
        public async Task<ActionResult<object>> CreatePromotionView([FromBody] CreatePromotionView create)
            => Ok(await _promotionService.CreatePromotionView(create));

        [HttpPut(nameof(UpdatePromotionStatus))]
        public async Task<ActionResult<object>> UpdatePromotionStatus([FromBody] UpdatePromotionStatusDto update)
            => Ok(await _promotionService.UpdatePromotionStatus(update));

        [HttpDelete(nameof(DeletePromotion))]
        public async Task<ActionResult<object>> DeletePromotion([FromQuery] int id)
            => Ok(await _promotionService.DeletePromotion(id));

        [HttpGet(nameof(GetAllService))]
        public async Task<ActionResult<ApplicationResult<List<OnAim.Admin.Domain.HubEntities.Service>>>> GetAllService()
            => Ok(await _promotionService.GetAllService());

        #endregion

        #region Promotion Template

        [HttpPost(nameof(CreatePromotionTemplate))]
        public async Task<ActionResult<object>> CreatePromotionTemplate([FromBody] CreatePromotionTemplateCommand command)
            => Ok(await Mediator.Send(command));

        [HttpGet(nameof(GetAllPromotionTemplates))]
        public async Task<ActionResult<ApplicationResult<PaginatedResult<PromotionTemplateListDto>>>> GetAllPromotionTemplates([FromQuery] BaseFilter filter)
            => Ok(await Mediator.Send(new GetAllPromotionTemplatesQuery(filter)));

        [HttpGet(nameof(GetPromotionTemplateById))]
        public async Task<ActionResult<ApplicationResult<PromotionTemplateListDto>>> GetPromotionTemplateById([FromQuery] string id)
            => Ok(await Mediator.Send(new GetPromotionTemplateByIdQuery(id)));

        [HttpDelete(nameof(DeletePromotionTemplate))]
        public async Task<ActionResult<object>> DeletePromotionTemplate([FromQuery] DeletePromotionTemplateCommand command)
            => Ok(await Mediator.Send(command));

        #endregion

        #region Promotion View Template

        [HttpGet(nameof(GetAllPromotionViewTemplates))]
        public async Task<ActionResult<ApplicationResult<PaginatedResult<PromotionViewTemplate>>>> GetAllPromotionViewTemplates([FromQuery] BaseFilter filter)
            => Ok(await Mediator.Send(new GetAllPromotionViewTemplatesQuery(filter)));

        [HttpGet(nameof(GetPromotionViewTemplateById))]
        public async Task<ActionResult<ApplicationResult<PromotionViewTemplate>>> GetPromotionViewTemplateById([FromQuery] string id)
            => Ok(await Mediator.Send(new GetPromotionViewTemplateByIdQuery(id)));

        [HttpPost(nameof(CreatePromotionViewTemplate))]
        public async Task<ActionResult<object>> CreatePromotionViewTemplate([FromBody] CreatePromotionViewTemplateCommand command)
            => Ok(await Mediator.Send(command));

        [HttpDelete(nameof(DeletePromotionViewTemplate))]
        public async Task<ActionResult<object>> DeletePromotionViewTemplate([FromQuery] DeletePromotionViewTemplateCommand command)
            => Ok(await Mediator.Send(command));

        #endregion


        [HttpGet(nameof(GetJsonFiles))]
        public async Task<ActionResult<object>> GetJsonFiles()
        {
            if (!Directory.Exists(_dataDirectory))
            {
                return NotFound("Data directory not found.");
            }

            var filesContent = new List<object>();
            var files = Directory.GetFiles(_dataDirectory, "*.json");

            foreach (var file in files)
            {
                try
                {
                    var content = await System.IO.File.ReadAllTextAsync(file);
                    var parsedJson = JsonSerializer.Deserialize<object>(content);

                    filesContent.Add(new
                    {
                        Service = Path.GetFileNameWithoutExtension(file),
                        Content = parsedJson
                    });
                }
                catch
                {
                    filesContent.Add(new
                    {
                        FileName = Path.GetFileNameWithoutExtension(file),
                        Content = "Invalid or unreadable JSON"
                    });
                }
            }

            return Ok(filesContent);
        }
    }
}