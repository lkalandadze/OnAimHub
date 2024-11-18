namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public sealed class UpdateLeaderboardScheduleDto
{
    public int Id { get; set; }
    public LeaderboardScheduleStatus Status { get; set; }
}
