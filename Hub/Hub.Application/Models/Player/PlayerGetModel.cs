using System.Text.Json.Serialization;

namespace Hub.Application.Models.Player;

public class PlayerGetModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }
}