using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Services.LeaderBoardServices;

public interface ILeaderBoardService
{
    Task<ApplicationResult<PaginatedResult<LeaderBoardListDto>>> GetAllLeaderBoard(LeaderBoardFilter? filter);
    Task<ApplicationResult<LeaderBoard.LeaderBoardData>> GetLeaderboardRecordById(int id);
    Task<ApplicationResult> GetAllPrizes();
    Task<ApplicationResult> CreateLeaderBoardRecord(CreateLeaderboardRecordCommand createLeaderboardRecordDto);
    Task<ApplicationResult> UpdateLeaderBoardRecord(UpdateLeaderboardRecordCommand updateLeaderboardRecordDto);
    Task<ApplicationResult> DeleteLeaderBoardRecord(DeleteLeaderboardRecordCommand delete);
    Task<ApplicationResult> GetCalendar(DateTimeOffset? startDate, DateTimeOffset? endDate);
    Task<ApplicationResult> GetLeaderboardSchedules(int? pageNumber, int? pageSize);
    Task<ApplicationResult> CreateLeaderboardSchedule(CreateLeaderboardScheduleCommand createLeaderboardSchedule);
    Task<ApplicationResult> UpdateLeaderboardSchedule(UpdateLeaderboardScheduleCommand updateLeaderboardSchedule);
}
