using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents.IntegrationEvents.Player;

namespace Hub.Application.Consumers.Player;

public class BanPlayerEventConsumer : IConsumer<BanPlayerEvent>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerBanRepository _playerBanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BanPlayerEventConsumer(IPlayerRepository playerRepository, IPlayerBanRepository playerBanRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _playerBanRepository = playerBanRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<BanPlayerEvent> context)
    {
        var player = await _playerRepository.Query().FirstOrDefaultAsync(x => x.Id == context.Message.PlayerId);

        if (player == default)
            throw new Exception("Player not found");

        var isPlayerBanned = await _playerBanRepository.Query().FirstOrDefaultAsync(x => x.PlayerId == context.Message.PlayerId && !x.IsRevoked);

        if (isPlayerBanned != default)
            throw new Exception("Player already banned");

        var playerBan = new PlayerBan(
            context.Message.PlayerId, 
            context.Message.ExpireDate,
            context.Message.IsPermanent, 
            context.Message.Description
            );

        player.Ban();

        _playerRepository.Update(player);
        await _playerBanRepository.InsertAsync(playerBan);
        await _unitOfWork.SaveAsync();

        await Task.CompletedTask;
    }
}
