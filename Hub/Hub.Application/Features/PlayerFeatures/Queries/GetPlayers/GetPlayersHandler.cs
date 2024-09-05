using Hub.Application.Features.PlayerFeatures.Dtos;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersHandler : IRequestHandler<GetPlayersQuery, GetPlayersResponse>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayersHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<GetPlayersResponse> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
    {
        var palyers = await _playerRepository.QueryAsync();

        //TODO: pagination

        var response = new GetPlayersResponse
        {
            Players = palyers?.Select(x => PlayerBaseDtoModel.MapFrom(x)),
        };

        return response;
    }
}