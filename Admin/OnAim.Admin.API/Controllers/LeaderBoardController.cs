using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllPrizes;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetCalendar;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardRecordById;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardSchedules;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Helpers;

namespace OnAim.Admin.API.Controllers;

public class LeaderBoardController : ApiControllerBase
{
    private readonly ILeaderBoardService _leaderBoardService;
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public LeaderBoardController(ILeaderBoardService leaderBoardService, ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderBoardService = leaderBoardService;
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    [HttpPost(nameof(CreateLeaderBoardRecord))]
    public async Task<IActionResult> CreateLeaderBoardRecord([FromBody] APP.CreateLeaderboardRecordCommand command)
    => Ok(await _leaderBoardService.CreateLeaderBoardRecord(command));

    [HttpPut(nameof(UpdateLeaderBoardRecord))]
    public async Task<IActionResult> UpdateLeaderBoardRecord([FromBody] UpdateLeaderboardRecordCommand command)
        => Ok(await _leaderBoardService.UpdateLeaderBoardRecord(command));

    [HttpGet(nameof(GetAllLeaderBoards))]
    public async Task<IActionResult> GetAllLeaderBoards([FromQuery] LeaderBoardFilter query)
        => Ok(await Mediator.Send(new GetAllLeaderBoardQuery(query)));    

    [HttpGet(nameof(GetLeaderboardRecordById))]
    public async Task<IActionResult> GetLeaderboardRecordById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetLeaderboardRecordByIdQuery(id)));

    [HttpPost(nameof(CreateLeaderboardSchedule))]
    public async Task<IActionResult> CreateLeaderboardSchedule([FromBody] APP.CreateLeaderboardScheduleCommand command)
        => Ok(await _leaderBoardService.CreateLeaderboardSchedule(command));

    [HttpPut(nameof(UpdateLeaderboardSchedule))]
    public async Task<IActionResult> UpdateLeaderboardSchedule([FromBody] APP.UpdateLeaderboardScheduleCommand command)
    => Ok(await _leaderBoardService.UpdateLeaderboardSchedule(command));

    [HttpGet(nameof(GetLeaderboardSchedules))]
    public async Task<IActionResult> GetLeaderboardSchedules([FromQuery] GetLeaderboardSchedulesQuery query)
        => Ok(await Mediator.Send(query));

    [HttpGet(nameof(GetLeaderboardRecordStatuses))]
    public ActionResult<List<EnumValueDto>> GetLeaderboardRecordStatuses()
    {
        var statuses = EnumHelper.GetEnumValues<APP.LeaderboardRecordStatus>();
        return Ok(statuses);
    }

    [HttpGet(nameof(GetAllPrizes))]
    public async Task<IActionResult> GetAllPrizes()
        => Ok(await Mediator.Send(new GetAllPrizesQuery())); 
    
    [HttpGet(nameof(GetCalendar))]
    public async Task<IActionResult> GetCalendar([FromQuery] GetCalendarQuery getCalendar)
        => Ok(await _leaderBoardService.GetCalendar(getCalendar.StartDate, getCalendar.EndDate));


    ///Template <summary>

    [HttpPost(nameof(CreateTemplate))]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateLeaderboardTemplateDto command)
        => Ok(await _leaderboardTemplateService.CreateLeaderboardTemplate(command));

    [HttpPut(nameof(UpdateTemplate))]
    public async Task<IActionResult> UpdateTemplate([FromBody] UpdateLeaderboardTemplateDto command)
        => Ok(await _leaderboardTemplateService.UpdateCoinTemplate(command));

    [HttpGet(nameof(GetAllTemplates))]
    public async Task<IActionResult> GetAllTemplates()
        => Ok(await _leaderboardTemplateService.GetAllLeaderboardTemplate());

    [HttpGet(nameof(GetLeaderboardTemplateById))]
    public async Task<IActionResult> GetLeaderboardTemplateById([FromQuery] string id)
    {
        await _leaderboardTemplateService.GetById(id);
        return Ok();
    }
}
