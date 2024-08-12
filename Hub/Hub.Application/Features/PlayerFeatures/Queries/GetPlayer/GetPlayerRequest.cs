using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;

public class GetPlayerRequest : IRequest<GetPlayerResponse>
{
    public int PlayerId { get; set; }
}