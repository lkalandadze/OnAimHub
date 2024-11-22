using Hub.Application.Features.PlayerBanFeatures.Commands.Update;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Abstractions;
using MediatR;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;

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
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player ban with the specified ID: [{request.Id}] was not found.");
        }

        if (playerBan.IsRevoked)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"Player with the specified ID: [{playerBan.PlayerId}] is already revoked.");
        }

        playerBan.Revoke();

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}