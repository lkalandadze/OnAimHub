namespace OnAim.Admin.Shared.DTOs.Segment;

public class SegmentListDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; }
    public int TotalPlayers { get; set; }
    public bool IsDeleted { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? LastUpdate { get; set; }
}
