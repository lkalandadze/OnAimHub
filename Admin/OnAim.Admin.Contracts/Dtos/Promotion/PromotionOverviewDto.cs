using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionOverviewDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<SegmentDto> Segments { get; set; }
    public List<GameDto> Games { get; set; }
    public List<LeaderBoardData> Leaderboards { get; set; }
}