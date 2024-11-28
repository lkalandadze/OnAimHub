using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardSchedule : BaseEntity<int>
{
    public string Name { get; set; } //Template name
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public DateOnly? SpecificDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardScheduleStatus Status { get; set; }
    public int LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }
}