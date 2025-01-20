using GameLib.Application.Generators;
using GameLib.Application.Holders;
using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Helpers;
using Shared.Lib.Wrappers;

namespace GameLib.Application.Services.Concrete;

public class GameConfigurationService : IGameConfigurationService
{
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly IPriceRepository _priceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EntityGenerator _entityGenerator;
    private readonly ConfigurationHolder _configurationHolder;

    public GameConfigurationService(
        IGameConfigurationRepository configurationRepository,
        IPriceRepository priceRepository,
        IUnitOfWork unitOfWork,
        EntityGenerator entityGenerator,
        ConfigurationHolder configurationHolder)
    {
        _configurationRepository = configurationRepository;
        _priceRepository = priceRepository;
        _unitOfWork = unitOfWork;
        _entityGenerator = entityGenerator;
        _configurationHolder = configurationHolder;
    }

    public Response<EntityMetadata?> GetConfigurationMetaData()
    {
        return new Response<EntityMetadata?> { Data = _entityGenerator.GenerateEntityMetadata(Globals.ConfigurationType) };
    }

    public async Task<Response<IEnumerable<GameConfiguration>>> GetAllAsync(int? configurationId, int? promotionId)
    {
        var configurations = _configurationRepository.Query();

        if (configurationId != null)
        {
            configurations = configurations.Where(x => x.Id == configurationId);
        }

        if (promotionId != null)
        {
            configurations = configurations.Where(x => x.PromotionId == promotionId);
        }

        return new Response<IEnumerable<GameConfiguration>>() { Data = await configurations.ToListAsync() };
    }

    public async Task<Response<GameConfiguration>> GetByIdAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Configuration with the specified ID: [{id}] was not found.");
        }

        return new Response<GameConfiguration> { Data = configuration };
    }

    public async Task CreateAsync(GameConfiguration configuration)
    {
        if (!CheckmateValidations.Checkmate.IsValid(configuration, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(configuration, true));
        }

        var existConfig = await _configurationRepository.Query(x => x.PromotionId == configuration.PromotionId)
                                                        .FirstOrDefaultAsync();

        if (existConfig != null)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"This game already has configuration for promotion with the specified ID: {configuration.PromotionId}");
        }

        try
        {
            ReflectionHelper.ReplacePropertyValuesDynamic(
                configuration,
                nameof(BasePrize.CoinId),
                (string coinId) => $"{configuration.PromotionId}_{coinId}");

            lock (_configurationHolder.GameConfigurations)
            {
                _configurationHolder.GameConfigurations.Add(configuration.PromotionId, configuration);
            }
            
            _configurationRepository.InsertConfigurationTree(configuration);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task UpdateAsync(GameConfiguration configuration)
    {
        if (!CheckmateValidations.Checkmate.IsValid(configuration, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(configuration, true));
        }

        var existConfig = await _configurationRepository.OfIdAsync(configuration.PromotionId);

        if (existConfig != null)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"This game already has configuration for promotion with the specified ID: {configuration.PromotionId}");
        }

        try
        {
            ReflectionHelper.ReplacePropertyValuesDynamic(
                configuration,
                nameof(BasePrize.CoinId),
                (string coinId) => $"{configuration.PromotionId}_{coinId}");

            await _configurationRepository.UpdateConfigurationTreeAsync(configuration);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task ActivateAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"Configuration with the specified ID: [{id}] was not found.");
        }

        configuration.Activate();

        _configurationRepository.Update(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"Configuration with the specified ID: [{id}] was not found.");
        }

        configuration.Deactivate();

        _configurationRepository.Update(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"Configuration with the specified ID: [{id}] was not found.");
        }

        foreach (var price in configuration.Prices)
        {
            _priceRepository.Delete(price);
        }

        _configurationRepository.DeleteConfigurationTree(configuration);
        await _unitOfWork.SaveAsync();
    }
}