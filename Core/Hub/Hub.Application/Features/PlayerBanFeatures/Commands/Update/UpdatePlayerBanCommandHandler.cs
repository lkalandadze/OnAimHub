using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.PlayerBanFeatures.Commands.Update;

public class UpdatePlayerBanCommandHandler : IRequestHandler<UpdatePlayerBanCommand>
{
    private readonly IPlayerBanRepository _playerBanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePlayerBanCommandHandler(IPlayerBanRepository playerBanRepository, IUnitOfWork unitOfWork)
    {
        _playerBanRepository = playerBanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdatePlayerBanCommand request, CancellationToken cancellationToken)
    {
        var playerBan = _playerBanRepository.Query().FirstOrDefault(x => x.Id == request.Id);

        if (playerBan == default)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player ban with the specified ID: [{request.Id}] was not found.");
        }

        playerBan.Update(request.ExpireDate, request.IsPermanent, request.Description);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}