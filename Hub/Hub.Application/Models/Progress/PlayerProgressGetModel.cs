using System.Text.Json.Serialization;

namespace Hub.Application.Models.Progress;

public class PlayerProgressGetModel
{
    [JsonPropertyName("progress")]
    public Dictionary<string, int>? Progress { get; set; }
}