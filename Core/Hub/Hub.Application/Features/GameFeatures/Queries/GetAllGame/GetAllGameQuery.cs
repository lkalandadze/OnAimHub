using MediatR;

namespace Hub.Application.Features.GameFeatures.Queries.GetAllGame;

public class GetAllGameQuery : IRequest<GetAllGameResponse>
{
    public GetAllGameQuery(bool isAuthorized = true)
    {
        IsAuthorized = isAuthorized;
    }

    public bool IsAuthorized { get; private set; }
    public string? Name { get; set; }
    public IEnumerable<string>? SegmentIds { get; set; }
    public int? PromotionId { get; set; }
}