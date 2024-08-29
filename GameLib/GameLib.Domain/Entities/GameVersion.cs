using Shared.Lib.Entities;

namespace GameLib.Domain.Entities;

public class GameVersion : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<int> SegmentIds { get; set; }

    public ICollection<Configuration> Configurations { get; set; }
}