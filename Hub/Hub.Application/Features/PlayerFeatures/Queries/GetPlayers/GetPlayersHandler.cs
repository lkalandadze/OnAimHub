 using Hub.Application.Features.PlayerFeatures.Dtos;
using Hub.Domain.Absractions.Repository;
using Shared.Lib.Extensions;
using MediatR;
using Shared.Lib.Wrappers;

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
        var players = _playerRepository.Query();

        var total = players.Count();

        var playerList = players.Pagination(request).ToList();

        var response = new GetPlayersResponse
        {
            Data = new PagedResponse<PlayerBaseDtoModel>
            (
                playerList?.Select(x => PlayerBaseDtoModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}