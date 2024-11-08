using Hub.Application.Models.Progress;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Absractions;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Services.Concrete;

public class PlayerProgressService : IPlayerProgressService
{
    private readonly IPlayerProgressRepository _playerProgressRepository;
    private readonly IPlayerProgressHistoryRepository _playerProgressHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionService _transactionService;

    public PlayerProgressService(IPlayerProgressRepository playerProgressRepository, IPlayerProgressHistoryRepository playerProgressHistoryRepository, IUnitOfWork unitOfWork, ITransactionService transactionService)
    {
        _playerProgressRepository = playerProgressRepository;
        _playerProgressHistoryRepository = playerProgressHistoryRepository;
        _unitOfWork = unitOfWork;
        _transactionService = transactionService;
    }

    public async Task InsertOrUpdateProgressesAsync(PlayerProgressGetModel progressModel, int playerId)
    {
        var currentProgresses = _playerProgressRepository.Query(x => x.PlayerId == playerId);

        foreach (var progress in progressModel.Progress!)
        {
            var currentProgress = currentProgresses.FirstOrDefault(x => x.CurrencyId == progress.Key);

            if (currentProgress == null)
            {
                var newProgress = new PlayerProgress(progress.Value, playerId, progress.Key);

                await _playerProgressRepository.InsertAsync(newProgress);
                await _transactionService.CreateTransactionAndApplyBalanceAsync(null, progress.Key, progress.Value, AccountType.Casino, AccountType.Player, TransactionType.Progress);
            }
            else
            {
                var newProgress = progress.Value - currentProgress.Progress;
                currentProgress.SetProgress(progress.Value);

                _playerProgressRepository.Update(currentProgress);

                if (newProgress > 0)
                {
                    await _transactionService.CreateTransactionAndApplyBalanceAsync(null, progress.Key, newProgress, AccountType.Casino, AccountType.Player, TransactionType.Progress);
                }
            }
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task ResetPlayerProgressesAndSaveHistoryAsync()
    {
        //TODO: performance

        var playerProgresses = await _playerProgressRepository.QueryAsync();

        _playerProgressRepository.DeleteAll();

        var playerProgressHistories = playerProgresses.Select(pp => new PlayerProgressHistory(pp.Progress, pp.PlayerId, pp.CurrencyId));

        await _playerProgressHistoryRepository.InsertRangeAsync(playerProgressHistories);
        await _unitOfWork.SaveAsync();
    }
}