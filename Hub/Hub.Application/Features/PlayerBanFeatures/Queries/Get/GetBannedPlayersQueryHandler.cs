using Hub.Application.Features.PlayerBanFeatures.Dtos;
using Hub.Application.Features.PlayerFeatures.Dtos;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;
using Hub.Domain.Absractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PlayerBanFeatures.Queries.Get;

public class GetBannedPlayersQueryHandler : IRequestHandler<GetBannedPlayersQuery, GetBannedPlayersQueryResponse>
{
    private readonly IPlayerBanRepository _playerBanRepository;

    public GetBannedPlayersQueryHandler(IPlayerBanRepository playerBanRepository)
    {
        _playerBanRepository = playerBanRepository;
    }

    public async Task<GetBannedPlayersQueryResponse> Handle(GetBannedPlayersQuery request, CancellationToken cancellationToken)
    {
        var players = _playerBanRepository.Query();

        var total = players.Count();

        var playerList = players.Pagination(request).ToList();

        var response = new GetBannedPlayersQueryResponse
        {
            Data = new PagedResponse<BannedPlayersDto>
            (
                playerList?.Select(x => BannedPlayersDto.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}