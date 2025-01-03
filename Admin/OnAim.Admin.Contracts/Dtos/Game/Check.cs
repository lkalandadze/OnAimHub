using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class Check
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("validationRule")]
    public string ValidationRule { get; set; }
}
