#nullable disable

using Wheel;

namespace Wheel.Application.Models.Game;

public class GameResponseModel
{
    public string Name { get; set; }
    public List<int> SegmentIds { get; set; }
    public DateTime ActivationTime { get; set; }
}