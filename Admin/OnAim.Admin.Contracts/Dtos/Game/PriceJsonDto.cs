using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class PriceJsonDto
{
    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("multiplier")]
    public decimal Multiplier { get; set; }

    [JsonPropertyName("coinId")]
    public string CoinId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
