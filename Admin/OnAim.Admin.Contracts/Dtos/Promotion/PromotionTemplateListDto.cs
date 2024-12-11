using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class PromotionTemplateListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<string> Segments { get; set; }
    public List<CoinsPromTempDto> Coins { get; set; }
    public List<LeaderboardsPromTempdto> Leaderboards { get; set; }
    public List<GameConfigurationPromTemplateListDto> Games { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}