using System.Text.Json;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameConfigurationTemplateDto
{
    public string Id { get; set; }
    public string GameName { get; set; }
    public JsonElement Configuration { get; set; }
}