using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class DataJsonDto
{
    [JsonPropertyName("rounds")]
    public List<RoundJsonDto> Rounds { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("promotionId")]
    public int PromotionId { get; set; }

    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; }

    [JsonPropertyName("fromTemplateId")]
    public string FromTemplateId { get; set; }

    [JsonPropertyName("prices")]
    public List<PriceJsonDto> Prices { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
