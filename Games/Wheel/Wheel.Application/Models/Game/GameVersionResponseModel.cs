#nullable disable

using Wheel;

namespace Wheel.Application.Models.Game;

public class GameVersionResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<int> SegmentIds { get; set; }
    public DateTime ActivationTime { get; set; }
}