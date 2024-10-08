#nullable disable

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Hub.Application.Models.Game;

public class GameConfigurationGetModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("segments")]
    public IEnumerable<GameSegmentGetModel> Segments { get; set; }
}