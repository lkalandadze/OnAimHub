namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public sealed class CreateLeaderboardScheduleDto
{
    public RepeatType RepeatType { get; set; }
    public int? RepeatValue { get; set; }
    public DateOnly? SpecificDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int LeaderboardTemplateId { get; set; }
}
