#nullable disable

using Wheel;

namespace Wheel.Application.Models.Game;

public class GameStatusModel
{
    public int GameId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public DateTime ActivationTime { get; set; }
    public bool IsActive { get; set; }
    public List<int> SegmentIds { get; set; }
}
