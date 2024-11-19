using Hub.Application.Features.PlayerBanFeatures.Commands.Update;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Abstractions;
using MediatR;

namespace Hub.Application.Features.PlayerBanFeatures.Commands.Revoke;

public class RevokePlayerBanCommandHandler : IRequestHandler<RevokePlayerBanCommand>
{
    private readonly IPlayerBanRepository _playerBanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevokePlayerBanCommandHandler(IPlayerBanRepository playerBanRepository, IUnitOfWork unitOfWork)
    {
        _playerBanRepository = playerBanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RevokePlayerBanCommand request, CancellationToken cancellationToken)
    {
        var playerBan = _playerBanRepository.Query().FirstOrDefault(x => x.Id == request.Id);

        if (playerBan == default)
            throw new Exception("Player ban not found");

        if (playerBan.IsRevoked)
            throw new Exception("Player already revoked");

        playerBan.Revoke();

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}