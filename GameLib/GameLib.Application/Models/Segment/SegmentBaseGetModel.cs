#nullable disable

namespace GameLib.Application.Models.Segment;

public class SegmentBaseGetModel
{
    public string Id { get; set; }
    public string Name { get; set; }

    public static SegmentBaseGetModel MapFrom(Domain.Entities.Segment segment)
    {
        return new SegmentBaseGetModel
        {
            Id = segment.Id,
            Name = segment.Id,
        };
    }
}