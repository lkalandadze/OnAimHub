using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class ConfigurationMetadataResponse
{
    [JsonPropertyName("data")]
    public ConfigurationMetadataData Data { get; set; }
}