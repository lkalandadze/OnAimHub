﻿using GameLib.Application.Managers;
using GameLib.Application.Models.Price;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Entities;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace GameLib.Application.Holders;

public class ConfigurationHolder
{
    internal Dictionary<int, GameConfiguration> GameConfigurations { get; private set; }

    private static object _sync = new();

    public ConfigurationHolder()
    {
        SetGameConfigurations();
    }

    public IEnumerable<BasePrizeGroup> GetPrizeGroups(int promotionId)
    {
        var gameConfiguration = GameConfigurations[promotionId];

        if (gameConfiguration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Game configuration with the specified promotion ID: [{promotionId}] was not found.");
        }

        var prizeGroups = gameConfiguration.GetType()
            .GetProperties()
            .Where(p => typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) && // Check if it's a collection
                        p.PropertyType != typeof(string)) // Exclude strings
            .Select(p => p.GetValue(gameConfiguration) as System.Collections.IEnumerable) // Get property value
            .Where(collection => collection != null) // Exclude null collections
            .SelectMany(collection => collection!.OfType<BasePrizeGroup>()) // Flatten and filter BasePrizeGroup types
            .ToList();

        return prizeGroups;
    }

    public IEnumerable<PriceBaseGetModel> GetPrices(int promotionId)
    {
        var gameConfiguration = GameConfigurations[promotionId];

        if (gameConfiguration == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Game configuration with the specified promotion ID: [{promotionId}] was not found.");
        }

        return gameConfiguration.Prices.Select(p => PriceBaseGetModel.MapFrom(p));
    }

    public T GetConfiguration<T>(int promotionId) where T : GameConfiguration
    {
        lock (_sync)
        {
            return (GameConfigurations.Where(c => c.Value.PromotionId == promotionId).FirstOrDefault().Value as T)!;
        }
    }

    public void ResetGameConfigurations()
    {
        lock (_sync)
        {
            GameConfigurations.Clear();
            SetGameConfigurations();
        }
    }

    private void SetGameConfigurations()
    {
        var gameConfigurations = RepositoryManager.GameConfigurationRepository().QueryAsync().Result;
        GameConfigurations = gameConfigurations.ToDictionary(config => config.PromotionId, config => config);
    }
}