using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameShortInfo
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("configurationCount")]
    public int ConfigurationCount { get; set; }

    [JsonPropertyName("promotionIds")]
    public List<int> PromotionIds { get; set; }
}
