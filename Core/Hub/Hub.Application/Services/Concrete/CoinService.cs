using Hub.Application.Models.Coin;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Services.Concrete;

public class CoinService : ICoinService
{
    private readonly IWithdrawOptionRepository _withdrawOptionRepository;
    private readonly IWithdrawOptionGroupRepository _withdrawOptionGroupRepository;

    public CoinService(IWithdrawOptionRepository withdrawOptionRepository, IWithdrawOptionGroupRepository withdrawOptionGroupRepository)
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
    }

    public async Task<IEnumerable<WithdrawOption>> GetWithdrawOptions(CreateOutCoinModel? outCoinModel)
    {
        if (outCoinModel == null)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.BusinessRuleViolation,
                "The provided model is null. Please ensure that a valid OutCoin model is supplied."
            );
        }

        var withdrawOptions = await _withdrawOptionRepository.QueryAsync(wo => outCoinModel.WithdrawOptionIds.Any(woId => woId == wo.Id));
        
        if (withdrawOptions == null || !withdrawOptions.Any())
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "No withdraw options were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing segments."
            );
        }

        return withdrawOptions;
    }

    public async Task<IEnumerable<WithdrawOptionGroup>> GetWithdrawOptionGroups(CreateOutCoinModel? outCoinModel)
    {
        if (outCoinModel == null)
        {
            throw new ApiException(
                ApiExceptionCodeTypes.BusinessRuleViolation,
                "The provided model is null. Please ensure that a valid OutCoin model is supplied."
            );
        }

        var withdrawOptionGroups = await _withdrawOptionGroupRepository.QueryAsync(wog => outCoinModel.WithdrawOptionGroupIds.Any(wogId => wogId == wog.Id));

        if (withdrawOptionGroups == null || !withdrawOptionGroups.Any())
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "No withdraw option groups were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing segments."
            );
        }

        return withdrawOptionGroups;
    }
}