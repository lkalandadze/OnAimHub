#nullable disable

namespace Wheel.Application.Models.Game;

public class GameResponseModel
{
    public string Name { get; set; }
    public List<int> SegmentIds { get; set; }
    public DateTimeOffset ActivationTime { get; set; }
}