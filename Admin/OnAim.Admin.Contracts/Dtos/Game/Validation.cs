using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class Validation
{
    [JsonPropertyName("checks")]
    public List<Check> Checks { get; set; }

    [JsonPropertyName("propertyPath")]
    public string PropertyPath { get; set; }

    [JsonPropertyName("memberSelector")]
    public string MemberSelector { get; set; }
}
