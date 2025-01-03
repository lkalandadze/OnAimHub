using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class Property
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("genericTypeMetadata")]
    public GenericTypeMetadata GenericTypeMetadata { get; set; }
}
