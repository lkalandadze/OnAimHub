#nullable disable

using System.Text.Json.Serialization;

namespace Hub.Application.Models.Game;

public class GameShortInfoGetModel
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("configurationCount")]
    public int ConfigurationCount { get; set; }

    [JsonPropertyName("segments")]
    public IEnumerable<string> Segments { get; set; }
}