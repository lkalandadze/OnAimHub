#nullable disable

using System.Text.Json.Serialization;

namespace Hub.Application.Models.Game;

public class GameSegmentGetModel
{

    [JsonPropertyName("id")]
    public string Id { get; set; }
}