using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardSchedule
{
    public int Id { get; set; }
    public string Title { get; set; }
    public RepeatType RepeatType { get; set; }
    public int? RepeatValue { get; set; }
    public LeaderboardScheduleStatus Status { get; set; }
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
}