using MediatR;

namespace Hub.Application.Features.GameFeatures.Queries.GetAllGame;

public class GetAllGameQuery : IRequest<GetAllGameResponse>
{
    public GetAllGameQuery(bool isAuthorized = true)
    {
        IsAuthorized = isAuthorized;
    }

    public bool IsAuthorized { get; set; }
}