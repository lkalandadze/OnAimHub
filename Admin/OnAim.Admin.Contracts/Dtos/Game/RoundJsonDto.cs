using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class RoundJsonDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("prizes")]
    public List<PrizeJsonDto> Prizes { get; set; }

    [JsonPropertyName("sequence")]
    public List<int> Sequence { get; set; }

    [JsonPropertyName("nextPrizeIndex")]
    public int NextPrizeIndex { get; set; }

    [JsonPropertyName("configurationId")]
    public int ConfigurationId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
