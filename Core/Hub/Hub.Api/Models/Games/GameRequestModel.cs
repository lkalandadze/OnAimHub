using Shared.Lib.Wrappers;

namespace Hub.Api.Models.Games;

public class GameRequestModel : PagedRequest
{
    public string? Name { get; set; }
    public int? PromotionId { get; set; }
}