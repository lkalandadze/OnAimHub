using GameLib.Application.Holders;
using PenaltyKicks.Application.Models.PenaltyKicks;
using PenaltyKicks.Application.Services.Abstract;
using PenaltyKicks.Domain.Abstractions.Repository;
using PenaltyKicks.Domain.Entities;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;
using GameLib.Domain.Abstractions;

namespace PenaltyKicks.Application.Services.Concrete;

public class PenaltyService : IPenaltyService
{
    private readonly IPenaltyConfigurationRepository _penaltyConfigurationRepository;
    private readonly ConfigurationHolder _configurationHolder;

    public PenaltyService(
        IPenaltyConfigurationRepository penaltyConfigurationRepository,
        ConfigurationHolder configurationHolder)
    {
        _penaltyConfigurationRepository = penaltyConfigurationRepository;
        _configurationHolder = configurationHolder;
    }

    public async Task<InitialDataResponseModel> GetInitialDataAsync(int promotionId)
    {
        return new InitialDataResponseModel()
        {
            Prices = _configurationHolder.GetPrices(promotionId).OrderBy(p => p.Bet),
            KicksCount = (_configurationHolder.GetConfiguration(promotionId) as PenaltyConfiguration)!.KicksCount
        };
    }
}