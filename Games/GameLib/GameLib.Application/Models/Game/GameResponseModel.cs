#nullable disable

namespace GameLib.Application.Models.Game;

public class GameResponseModel
{
    public string Name { get; set; }
    public DateTimeOffset ActivationTime { get; set; }
}