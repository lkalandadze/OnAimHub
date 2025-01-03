using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class ConfigurationMetadataData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("validations")]
    public List<Validation> Validations { get; set; }

    [JsonPropertyName("properties")]
    public List<Property> Properties { get; set; }
}
