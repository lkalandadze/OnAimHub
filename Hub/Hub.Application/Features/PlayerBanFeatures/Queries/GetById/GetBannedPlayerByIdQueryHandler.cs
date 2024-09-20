using Hub.Application.Features.PlayerBanFeatures.Dtos;
using Hub.Domain.Absractions.Repository;
using MediatR;

namespace Hub.Application.Features.PlayerBanFeatures.Queries.GetById;

public class GetBannedPlayerByIdQueryHandler : IRequestHandler<GetBannedPlayerByIdQuery, GetBannedPlayerByIdQueryResponse>
{
    private readonly IPlayerBanRepository _playerBanRepository;
    public GetBannedPlayerByIdQueryHandler(IPlayerBanRepository playerBanRepository)
    {
        _playerBanRepository = playerBanRepository;
    }

    public async Task<GetBannedPlayerByIdQueryResponse> Handle(GetBannedPlayerByIdQuery request, CancellationToken cancellationToken)
    {
        var bannedPlayer = _playerBanRepository.Query().FirstOrDefault(x => x.Id == request.Id);

        if (bannedPlayer == default)
            throw new Exception("Banned player not found");

        var bannedPlayerDto = BannedPlayersDto.MapFrom(bannedPlayer);

        var response = new GetBannedPlayerByIdQueryResponse
        {
            Data = bannedPlayerDto
        };

        return response;
    }
}