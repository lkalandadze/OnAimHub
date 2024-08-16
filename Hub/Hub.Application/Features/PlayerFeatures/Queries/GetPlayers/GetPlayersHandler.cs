using Hub.Application.Features.PlayerFeatures.Dtos;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;

public class GetPlayersHandler : IRequestHandler<GetPlayersRequest, GetPlayersResponse>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayersHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<GetPlayersResponse> Handle(GetPlayersRequest request, CancellationToken cancellationToken)
    {
        var slotTransactions = await _playerRepository.QueryAsync();

        var response = new GetPlayersResponse
        {
            Players = slotTransactions?.Select(x => PlayerBaseDtoModel.MapFrom(x)),
        };

        return response;
    }
}