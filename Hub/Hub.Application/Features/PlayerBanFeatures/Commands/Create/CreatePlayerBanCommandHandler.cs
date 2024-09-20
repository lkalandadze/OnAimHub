﻿using Hub.Domain.Absractions;
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

        var playerBan = new PlayerBan(request.PlayerId, request.ExpireDate, request.IsPermanent, request.Description);

        await _playerBanRepository.InsertAsync(playerBan);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}