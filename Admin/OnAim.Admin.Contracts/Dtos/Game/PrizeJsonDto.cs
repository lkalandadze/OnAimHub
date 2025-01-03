using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class PrizeJsonDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("wheelIndex")]
    public int WheelIndex { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("probability")]
    public int Probability { get; set; }

    [JsonPropertyName("coinId")]
    public string CoinId { get; set; }

    [JsonPropertyName("prizeGroupId")]
    public int PrizeGroupId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
