using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

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
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{request.PlayerId}] was not found.");
        }

        var isPlayerBanned = _playerBanRepository.Query().FirstOrDefault(x => x.PlayerId == request.PlayerId && !x.IsRevoked);

        if (isPlayerBanned != default)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"Promotion with the specified ID: [{request.PlayerId}] is already banned.");
        }

        var playerBan = new PlayerBan(request.PlayerId, request.ExpireDate, request.IsPermanent, request.Description);

        player.Ban();

        _playerRepository.Update(player);
        await _playerBanRepository.InsertAsync(playerBan);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}