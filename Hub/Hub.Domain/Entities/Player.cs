using Shared.Lib.Entities;

namespace Hub.Domain.Entities;

public class Player : BaseEntity
{
    public override int Id { get; set; }
    public string UserName { get; set; }
    public List<int> SegmentIds { get; set; }
}