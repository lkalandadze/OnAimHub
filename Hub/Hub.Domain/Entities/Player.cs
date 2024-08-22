using Hub.Domain.Absractions;

namespace Hub.Domain.Entities;

public class Player : BaseEntity
{
    public override int Id { get; set; }
    public string UserName { get; set; }
    public int SegmentId { get; set; }
}