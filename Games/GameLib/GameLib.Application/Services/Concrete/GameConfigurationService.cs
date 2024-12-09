using GameLib.Application.Generators;
using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace GameLib.Application.Services.Concrete;

public class GameConfigurationService : IGameConfigurationService
{
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly IPriceRepository _priceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EntityGenerator _entityGenerator;

    public GameConfigurationService(
        IGameConfigurationRepository configurationRepository,
        IPriceRepository priceRepository,
        IUnitOfWork unitOfWork,
        EntityGenerator entityGenerator)
    {
        _configurationRepository = configurationRepository;
        _priceRepository = priceRepository;
        _unitOfWork = unitOfWork;
        _entityGenerator = entityGenerator;
    }

    public EntityMetadata? GetConfigurationMetaData()
    {
        return _entityGenerator.GenerateEntityMetadata(Globals.ConfigurationType);
    }

    public async Task<IEnumerable<ConfigurationBaseGetModel>> GetAllAsync()
    {
        var configurations = await _configurationRepository.Query().ToListAsync();
        var mappedConfigurations = configurations.Select(c => ConfigurationBaseGetModel.MapFrom(c));

        return mappedConfigurations == null ? Enumerable.Empty<ConfigurationBaseGetModel>() : mappedConfigurations;
    }

    public async Task<GameConfiguration> GetByIdAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Configuration with the specified ID: [{id}] was not found.");
        }

        return configuration;
    }

    public async Task CreateAsync(GameConfiguration configuration)
    {
        try
        {
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
        try
        {
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