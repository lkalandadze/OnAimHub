using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;

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
            throw new Exception("Player ban not found");

        playerBan.Update(request.ExpireDate, request.IsPermanent, request.Description);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}