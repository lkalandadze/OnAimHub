using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Services.Concrete;

public class PlayerBalanceService : IPlayerBalanceService
{
    private readonly IPlayerBalanceRepository _playerBalanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlayerBalanceService(IPlayerBalanceRepository playerBalanceRepository, IUnitOfWork unitOfWork)
    {
        _playerBalanceRepository = playerBalanceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PlayerBalance> GetOrCreatePlayerBalanceAsync(int playerId, string coinId, int promotionId)
    {
        var playerBalance = (await _playerBalanceRepository.QueryAsync(x => x.PlayerId == playerId &&
                                                                            x.CoinId == coinId))
                                                           .FirstOrDefault();

        if (playerBalance == null)
        {
            playerBalance = new PlayerBalance(0, playerId, coinId, promotionId);

            await _playerBalanceRepository.InsertAsync(playerBalance);

            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            
        }

        return playerBalance;
    }

    public async Task ApplyPlayerBalanceOperationAsync(int playerId, string coinId, AccountType fromAccount, AccountType toAccount, decimal amount, int promotionId)
    {
        var balance = await GetOrCreatePlayerBalanceAsync(playerId, coinId, promotionId);

        if (fromAccount == AccountType.Player)
        {
            if (amount > balance.Amount)
            {
                throw new ApiException(ApiExceptionCodeTypes.InsufficientFunds, $"Player with ID {playerId} does not have enough balance to perform this operation.");
            }

            balance.SetAmount(balance.Amount - amount);
        }
        else if (toAccount == AccountType.Player)
        {
            balance.SetAmount(balance.Amount + amount);
        }

        _playerBalanceRepository.Update(balance);
        await _unitOfWork.SaveAsync();
    }

    public async Task ResetBalancesByCoinIdAsync(string coinId)
    {
        var balances = _playerBalanceRepository.Query().Where(x => x.CoinId == coinId);

        foreach (var balance in balances)
        {
            balance.SetAmount(0);
        }

        await _unitOfWork.SaveAsync();
    }
}