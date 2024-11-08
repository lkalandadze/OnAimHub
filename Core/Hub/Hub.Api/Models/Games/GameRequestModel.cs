namespace Hub.Api.Models.Games;

public class GameRequestModel
{
    public string? Name { get; set; }
    public IEnumerable<string>? SegmentIds { get; set; }
}