namespace OnAim.Admin.APP.Services.LeaderBoardServices;

public interface ILeaderBoardService
{
    Task<ApplicationResult<PaginatedResult<LeaderBoardListDto>>> GetAllLeaderBoard(LeaderBoardFilter? filter);
    Task<ApplicationResult<LeaderBoard.LeaderBoardData>> GetLeaderboardRecordById(int id);
    Task<ApplicationResult<object>> GetAllPrizes();
    Task<ApplicationResult<bool>> CreateLeaderBoardRecord(CreateLeaderboardRecordCommand createLeaderboardRecordDto);
    Task<ApplicationResult<bool>> UpdateLeaderBoardRecord(UpdateLeaderboardRecordCommand updateLeaderboardRecordDto);
    Task<ApplicationResult<bool>> DeleteLeaderBoardRecord(DeleteLeaderboardRecordCommand delete);
    Task<ApplicationResult<object>> GetCalendar(DateTimeOffset? startDate, DateTimeOffset? endDate);
    Task<ApplicationResult<object>> GetLeaderboardSchedules(int? pageNumber, int? pageSize);
    Task<ApplicationResult<bool>> CreateLeaderboardSchedule(CreateLeaderboardScheduleCommand createLeaderboardSchedule);
    Task<ApplicationResult<bool>> UpdateLeaderboardSchedule(UpdateLeaderboardScheduleCommand updateLeaderboardSchedule);

    Task<object> UpsertProgress(int leaderboardRecordId, int generatedAmount);
    Task<object> FinishLeaderboard(int leaderboardRecordId);
    Task<object> GetLeaderboardProgress(int leaderboardRecordId, int pageNumber, int pageSize);
    Task<object> GetLeaderboardProgressForUser(int playerId, int pageNumber, int pageSize);
    Task<object> GetLeaderboardResults(int leaderboardRecordId, int pageNumber, int pageSize);
    Task<object> GetLeaderboardByPlayerIdResults(int playerId, int pageNumber, int pageSize);
}
