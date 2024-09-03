using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class GameVersion : BaseEntity<int>
{
    public GameVersion(string name, bool isActive, List<int> segmentIds)
    {
        Name = name;
        IsActive = isActive;
        SegmentIds = segmentIds;
    }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<int> SegmentIds { get; set; }

    public ICollection<Configuration> Configurations { get; set; }

    public void Update(string name, bool isActive, List<int> segmentIds)
    {
        Name = name; 
        IsActive = isActive; 
        SegmentIds = segmentIds;
    }
}