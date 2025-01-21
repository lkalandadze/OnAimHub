using System.Text.Json;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameConfigPromDto
{
    public string GameName { get; set; }
    public JsonElement CreateGame { get; set; }
}