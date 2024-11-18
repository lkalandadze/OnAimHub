using MongoDB.Bson;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class CreatePromotionDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartIn { get; set; }
    public DateTimeOffset EndIn { get; set; }
    public List<SegmentDto>? Segments { get; set; }
    //public CoinInDto CoinInDto { get; set; }
    public CoinOutDto CoinOutDto { get; set; }
}
public class CoinInDto
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
}
public class CoinOutDto
{
    //public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
}