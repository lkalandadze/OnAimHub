namespace Wheel.Application.Models;

public class GameVersionResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<int> SegmentIds { get; set; }
    public DateTime ActivationTime { get; set; }
}