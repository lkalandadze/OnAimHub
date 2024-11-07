using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.Services.LeaderBoard;

namespace OnAim.Admin.APP.Services.Abstract;

public interface ILeaderBoardService
{
    Task<ApplicationResult> GetLeaderBoardTemplates(BaseFilter filter);
    Task<ApplicationResult> GetLeaderboardTemplateById(int id);
    Task<ApplicationResult> GetAllLeaderBoard(LeaderBoardFilter? filter);
    Task<ApplicationResult> GetLeaderboardRecordById(int id);
    Task<ApplicationResult> GetAllPrizes();
    Task<ApplicationResult> CreateTemplate(CreateLeaderboardTemplateDto createLeaderboardTemplateDto);
    Task<ApplicationResult> UpdateTemplate(UpdateLeaderboardTemplateDto updateLeaderboardTemplateDto);
    Task<ApplicationResult> CreateLeaderBoardRecord(CreateLeaderboardRecordDto createLeaderboardRecordDto);
    Task<ApplicationResult> UpdateLeaderBoardRecord(UpdateLeaderboardRecordDto updateLeaderboardRecordDto);
    Task<ApplicationResult> GetCalendar(DateTimeOffset? startDate, DateTimeOffset? endDate);
    Task<ApplicationResult> GetLeaderboardSchedules(int? pageNumber, int? pageSize);
    Task<ApplicationResult> CreateLeaderboardSchedule(CreateLeaderboardScheduleDto createLeaderboardSchedule);
    Task<ApplicationResult> UpdateLeaderboardSchedule(UpdateLeaderboardScheduleDto updateLeaderboardSchedule);
}
