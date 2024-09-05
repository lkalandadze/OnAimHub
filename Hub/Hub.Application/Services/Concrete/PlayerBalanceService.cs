﻿using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
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

    public async Task<PlayerBalance> GetOrCreatePlayerBalanceAsync(int playerId, string currencyId)
    {
        var playerBalance = (await _playerBalanceRepository.QueryAsync(x => x.PlayerId == playerId &&
                                                                            x.CurrencyId == currencyId))
                                                           .FirstOrDefault();

        if (playerBalance == null)
        {
            playerBalance = new PlayerBalance
            {
                Amount = 0,
                CurrencyId = currencyId,
                PlayerId = playerId,
            };

            await _playerBalanceRepository.InsertAsync(playerBalance);
            await _unitOfWork.SaveAsync();
        }

        return playerBalance;
    }

    public async Task ApplyPlayerBalanceOperationAsync(int playerId, string currencyId, AccountType fromAccount, AccountType toAccount, decimal amount)
    {
        var balance = await GetOrCreatePlayerBalanceAsync(playerId, currencyId);

        if (fromAccount == AccountType.Player)
        {
            if (amount > balance.Amount)
            {
                throw new ApiException(ApiExceptionCodeTypes.InsufficientFunds, $"Player with ID {playerId} does not have enough balance to perform this operation.");
            }

            balance.Amount -= amount;
        }
        else if (toAccount == AccountType.Player)
        {
            balance.Amount += amount;
        }

        _playerBalanceRepository.Update(balance);
        await _unitOfWork.SaveAsync();
    }
}