using Hub.Application.Features.PlayerFeatures.Dtos;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;

public class GetPlayerHandler : IRequestHandler<GetPlayerQuery, GetPlayerResponse>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<GetPlayerResponse> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.OfIdAsync(request.PlayerId);

        if (player == null)
        {
            throw new KeyNotFoundException($"Player was not found for Id: {request.PlayerId}");
        }

        return new GetPlayerResponse()
        {
            Player = PlayerBaseDtoModel.MapFrom(player),
        };
    }
}