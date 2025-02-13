﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Create;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Update;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Create;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Update;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllPrizes;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetCalendar;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardRecordById;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardSchedules;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Update;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetById;
using OnAim.Admin.APP.Services.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Helpers;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.API.Controllers;

public class LeaderBoardController : ApiControllerBase
{
    #region LeaderBoard
    [HttpPost(nameof(CreateLeaderBoardRecord))]
    public async Task<ActionResult<object>> CreateLeaderBoardRecord([FromBody] CreateLeaderboardCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPut(nameof(UpdateLeaderBoardRecord))]
    public async Task<ActionResult<object>> UpdateLeaderBoardRecord([FromBody] UpdateLeaderBoardCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetAllLeaderBoards))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<LeaderBoardListDto>>>> GetAllLeaderBoards([FromQuery] LeaderBoardFilter query)
        => Ok(await Mediator.Send(new GetAllLeaderBoardQuery(query)));    

    [HttpGet(nameof(GetLeaderboardRecordById))]
    public async Task<ActionResult<ApplicationResult<APP.Services.LeaderBoard.LeaderBoardData>>> GetLeaderboardRecordById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetLeaderboardRecordByIdQuery(id)));

    [HttpPost(nameof(CreateLeaderboardSchedule))]
    public async Task<ActionResult<object>> CreateLeaderboardSchedule([FromBody] CreateScheduleCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPut(nameof(UpdateLeaderboardSchedule))]
    public async Task<ActionResult<object>> UpdateLeaderboardSchedule([FromBody] UpdateScheduleCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetLeaderboardSchedules))]
    public async Task<ActionResult<object>> GetLeaderboardSchedules([FromQuery] GetLeaderboardSchedulesQuery query)
        => Ok(await Mediator.Send(query));

    [HttpGet(nameof(GetLeaderboardRecordStatuses))]
    public ActionResult<List<EnumValueDto>> GetLeaderboardRecordStatuses()
        => Ok(EnumHelper.GetEnumValues<APP.LeaderboardRecordStatus>());

    [HttpGet(nameof(GetAllPrizes))]
    public async Task<ActionResult<object>> GetAllPrizes()
        => Ok(await Mediator.Send(new GetAllPrizesQuery())); 
    
    [HttpGet(nameof(GetCalendar))]
    public async Task<ActionResult<object>> GetCalendar([FromQuery] GetCalendarQuery getCalendar)
        => Ok(await Mediator.Send(getCalendar));
    #endregion

    #region LeaderBoard Template

    [HttpPost(nameof(CreateLeaderBoardTemplate))]
    public async Task<ActionResult<object>> CreateLeaderBoardTemplate([FromBody] CreateLeaderboardTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpPut(nameof(UpdateLeaderBoardTemplate))]
    public async Task<ActionResult<object>> UpdateLeaderBoardTemplate([FromBody] UpdateLeaderboardTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpDelete(nameof(DeleteLeaderBoardTemplate))]
    public async Task<ActionResult<object>> DeleteLeaderBoardTemplate([FromQuery] DeleteLeaderboardTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetAllLeaderBoardTemplates))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<LeaderBoardTemplateListDto>>>> GetAllLeaderBoardTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllLeaderboardTemplatesQuery(filter)));

    [HttpGet(nameof(GetLeaderboardTemplateById))]
    public async Task<ActionResult<ApplicationResult<LeaderBoardTemplateListDto>>> GetLeaderboardTemplateById([FromQuery] GetLeaderboardTemplateByIdQuery query)
        => Ok(await Mediator.Send(query));
    #endregion
}
