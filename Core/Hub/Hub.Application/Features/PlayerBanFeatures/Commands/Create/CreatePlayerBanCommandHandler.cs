using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.PlayerBanFeatures.Commands.Create;

public class CreatePlayerBanCommandHandler : IRequestHandler<CreatePlayerBanCommand>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerBanRepository _playerBanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlayerBanCommandHandler(IPlayerRepository playerRepository, IPlayerBanRepository playerBanRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _playerBanRepository = playerBanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreatePlayerBanCommand request, CancellationToken cancellationToken)
    {
        var player = _playerRepository.Query().FirstOrDefault(x => x.Id == request.PlayerId);

        if (player == default)
            throw new Exception("Player not found");

        var isPlayerBanned = _playerBanRepository.Query().FirstOrDefault(x => x.PlayerId == request.PlayerId && !x.IsRevoked);

        if (isPlayerBanned != default)
            throw new Exception("Player already banned");

        var playerBan = new PlayerBan(request.PlayerId, request.ExpireDate, request.IsPermanent, request.Description);

        player.Ban();

        _playerRepository.Update(player);
        await _playerBanRepository.InsertAsync(playerBan);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}