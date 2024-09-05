using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class GameVersion : BaseEntity<int>
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<int> SegmentIds { get; set; }

    public ICollection<Configuration> Configurations { get; set; }
}