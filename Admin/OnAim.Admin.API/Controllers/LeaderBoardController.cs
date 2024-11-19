using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllPrizes;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllTemplates;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetCalendar;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardRecordById;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardSchedules;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardTemplateById;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.LeaderBoard;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Helpers;

namespace OnAim.Admin.API.Controllers;

public class LeaderBoardController : ApiControllerBase
{
    private readonly ILeaderBoardService _leaderBoardService;

    public LeaderBoardController(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    [HttpGet(nameof(GetAllTemplates))]
    public async Task<IActionResult> GetAllTemplates([FromQuery] BaseFilter query)
        => Ok(await Mediator.Send(new GetAllTemplatesQuery(query)));

    [HttpGet(nameof(GetLeaderboardTemplateById))]
    public async Task<IActionResult> GetLeaderboardTemplateById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetLeaderboardTemplateByIdQuery(id)));

    [HttpGet(nameof(GetAllLeaderBoards))]
    public async Task<IActionResult> GetAllLeaderBoards([FromQuery] LeaderBoardFilter query)
        => Ok(await Mediator.Send(new GetAllLeaderBoardQuery(query)));    

    [HttpGet(nameof(GetLeaderboardRecordById))]
    public async Task<IActionResult> GetLeaderboardRecordById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetLeaderboardRecordByIdQuery(id)));

    [HttpPost(nameof(CreateTemplate))]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateLeaderboardTemplateCommand command)
        => Ok(await _leaderBoardService.CreateTemplate(command));

    [HttpPut(nameof(UpdateTemplate))]
    public async Task<IActionResult> UpdateTemplate([FromBody] UpdateLeaderboardTemplateCommand command)
        => Ok(await _leaderBoardService.UpdateTemplate(command));

    [HttpPost(nameof(CreateLeaderBoardRecord))]
    public async Task<IActionResult> CreateLeaderBoardRecord([FromBody] APP.CreateLeaderboardRecordCommand command)
        => Ok(await _leaderBoardService.CreateLeaderBoardRecord(command));   

    [HttpPost(nameof(CreateLeaderboardSchedule))]
    public async Task<IActionResult> CreateLeaderboardSchedule([FromBody] APP.CreateLeaderboardScheduleCommand command)
        => Ok(await _leaderBoardService.CreateLeaderboardSchedule(command));

    [HttpPost(nameof(UpdateLeaderboardSchedule))]
    public async Task<IActionResult> UpdateLeaderboardSchedule([FromBody] APP.UpdateLeaderboardScheduleCommand command)
    => Ok(await _leaderBoardService.UpdateLeaderboardSchedule(command));

    [HttpPut(nameof(UpdateLeaderBoardRecord))]
    public async Task<IActionResult> UpdateLeaderBoardRecord([FromBody] UpdateLeaderboardRecordCommand command)
        => Ok(await _leaderBoardService.UpdateLeaderBoardRecord(command));

    [HttpGet(nameof(GetLeaderboardRecordStatuses))]
    public ActionResult<List<EnumValueDto>> GetLeaderboardRecordStatuses()
    {
        var statuses = EnumHelper.GetEnumValues<Contracts.Dtos.LeaderBoard.LeaderboardRecordStatus>();
        return Ok(statuses);
    }

    [HttpGet(nameof(GetAllPrizes))]
    public async Task<IActionResult> GetAllPrizes()
        => Ok(await Mediator.Send(new GetAllPrizesQuery())); 
    
    [HttpGet(nameof(GetCalendar))]
    public async Task<IActionResult> GetCalendar([FromQuery] GetCalendarQuery getCalendar)
        => Ok(await Mediator.Send(getCalendar));  
    
    [HttpGet(nameof(GetLeaderboardSchedules))]
    public async Task<IActionResult> GetLeaderboardSchedules([FromQuery] GetLeaderboardSchedulesQuery query)
        => Ok(await Mediator.Send(query));
}
