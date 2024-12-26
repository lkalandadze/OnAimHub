using OnAim.Admin.Contracts.Dtos.LeaderBoard;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionLeaderboardDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Place { get; set; }
    public RepeatType RepeatType { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}
