using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Execute;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Create;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Update;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Template.CreateTemplate;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Template.UpdateTemplate;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllPrizes;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllTemplates;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Helpers;

namespace OnAim.Admin.API.Controllers;

public class LeaderBoardController : ApiControllerBase
{
    [HttpGet(nameof(GetAllTemplates))]
    public async Task<IActionResult> GetAllTemplates([FromQuery] GetAllTemplatesQuery query)
        => Ok(await Mediator.Send(query));

    [HttpGet(nameof(GetAllLeaderBoards))]
    public async Task<IActionResult> GetAllLeaderBoards([FromQuery] GetAllLeaderBoardQuery query)
        => Ok(await Mediator.Send(query));

    [HttpPost(nameof(CreateTemplate))]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateLeaderboardTemplateDto command)
        => Ok(await Mediator.Send(new CreateTemplateCommand(command)));

    [HttpPut(nameof(UpdateTemplate))]
    public async Task<IActionResult> UpdateTemplate([FromBody] UpdateLeaderboardTemplateDto command)
        => Ok(await Mediator.Send(new UpdateTemplateCommand(command)));


    [HttpPost(nameof(CreateLeaderBoardRecord))]
    public async Task<IActionResult> CreateLeaderBoardRecord([FromBody] CreateLeaderboardRecordDto command)
        => Ok(await Mediator.Send(new CreateLeaderboardRecordCommand(command)));

    [HttpPut(nameof(UpdateLeaderBoardRecord))]
    public async Task<IActionResult> UpdateLeaderBoardRecord([FromBody] UpdateLeaderboardRecordDto command)
        => Ok(await Mediator.Send(new UpdateLeaderBoardRecordCommand(command)));

    [HttpPost(nameof(CreateSchedule) + "/{templateId}")]
    public async Task<IActionResult> CreateSchedule([FromRoute] int templateId)
        => Ok(await Mediator.Send(new CreateScheduleCommand(templateId)));

    [HttpPost(nameof(Execute) + "/{templateId}")]
    public async Task<IActionResult> Execute([FromRoute] int templateId)
        => Ok(await Mediator.Send(new ExecuteCommand(templateId)));

    [HttpGet(nameof(GetLeaderboardRecordStatuses))]
    public ActionResult<List<EnumValueDto>> GetLeaderboardRecordStatuses()
    {
        var statuses = EnumHelper.GetEnumValues<LeaderboardRecordStatus>();
        return Ok(statuses);
    }

    [HttpGet(nameof(GetAllPrizes))]
    public async Task<IActionResult> GetAllPrizes()
        => Ok(await Mediator.Send(new GetAllPrizesQuery()));
}
