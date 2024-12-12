﻿using GameLib.Application.Generators;
using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Wrappers;

namespace GameLib.Application.Services.Concrete;

public class GameConfigurationService : IGameConfigurationService
{
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly IPriceRepository _priceRepository;
    private readonly ICoinRepository _coinRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EntityGenerator _entityGenerator;

    public GameConfigurationService(
        IGameConfigurationRepository configurationRepository,
        IPriceRepository priceRepository,
        ICoinRepository coinRepository,
        IUnitOfWork unitOfWork,
        EntityGenerator entityGenerator)
    {
        _configurationRepository = configurationRepository;
        _priceRepository = priceRepository;
        _coinRepository = coinRepository;
        _unitOfWork = unitOfWork;
        _entityGenerator = entityGenerator;
    }

    public Response<EntityMetadata?> GetConfigurationMetaData()
    {
        return new Response<EntityMetadata?> { Data = _entityGenerator.GenerateEntityMetadata(Globals.ConfigurationType) };
    }

    public async Task<Response<IEnumerable<ConfigurationBaseGetModel>>> GetAllAsync()
    {
        var configurations = await _configurationRepository.Query().ToListAsync();
        var mappedConfigurations = configurations.Select(c => ConfigurationBaseGetModel.MapFrom(c));

        return new Response<IEnumerable<ConfigurationBaseGetModel>>() { Data = mappedConfigurations };
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
        if (!CheckmateValidations.Checkmate.IsValid(configuration, true))
        {
            throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(configuration, true));
        }

        
        try
        {
            var price = configuration.Prices.FirstOrDefault()!;
            await _coinRepository.InsertAsync(new Coin() { Id = $"{configuration.Id}_{price.CoinId}", Name = price.CoinId });
       
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