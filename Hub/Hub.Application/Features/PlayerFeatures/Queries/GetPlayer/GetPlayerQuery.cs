using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;

public class GetPlayerQuery : IRequest<GetPlayerResponse>
{
    public int PlayerId { get; set; }
}