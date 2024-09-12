#nullable disable

namespace Wheel.Application.Models.Game;

public class GameResponseModel
{
    public string Name { get; set; }
    public List<string> SegmentIds { get; set; }
    public DateTimeOffset ActivationTime { get; set; }
}