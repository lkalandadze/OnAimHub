using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Services.LeaderBoardServices;

public interface ILeaderBoardService
{
    //Task<ApplicationResult> GetLeaderBoardTemplates(BaseFilter filter);
    //Task<ApplicationResult> GetLeaderboardTemplateById(int id);
    Task<ApplicationResult> GetAllLeaderBoard(LeaderBoardFilter? filter);
    Task<ApplicationResult> GetLeaderboardRecordById(int id);
    Task<ApplicationResult> GetAllPrizes();
    Task<ApplicationResult> CreateLeaderBoardRecord(CreateLeaderboardRecordCommand createLeaderboardRecordDto);
    Task<ApplicationResult> UpdateLeaderBoardRecord(UpdateLeaderboardRecordCommand updateLeaderboardRecordDto);
    Task<ApplicationResult> GetCalendar(DateTimeOffset? startDate, DateTimeOffset? endDate);
    Task<ApplicationResult> GetLeaderboardSchedules(int? pageNumber, int? pageSize);
    Task<ApplicationResult> CreateLeaderboardSchedule(CreateLeaderboardScheduleCommand createLeaderboardSchedule);
    Task<ApplicationResult> UpdateLeaderboardSchedule(UpdateLeaderboardScheduleCommand updateLeaderboardSchedule);
}
