using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.GameFeatures.Queries.GetAllGame;

public class GetAllGameQuery : PagedRequest, IRequest<GetAllGameResponse>
{
    public GetAllGameQuery(bool isAuthorized = true)
    {
        IsAuthorized = isAuthorized;
    }

    public bool IsAuthorized { get; private set; }
    public string? Name { get; set; }
    public int? PromotionId { get; set; }
}