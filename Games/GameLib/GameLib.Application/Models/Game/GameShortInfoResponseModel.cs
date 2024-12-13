#nullable disable

namespace GameLib.Application.Models.Game;

public class GameShortInfoResponseModel
{
    public bool Status { get; set; }
    public string Description { get; set; }
    public int ConfigurationCount { get; set; }
    public IEnumerable<int> PromotionIds { get; set; }
}