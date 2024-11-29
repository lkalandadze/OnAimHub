using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardSchedule : BaseEntity<int>
{
    public string Title { get; set; }  //R
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public LeaderboardScheduleStatus Status { get; set; } //R? Need logic if we need it
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
}