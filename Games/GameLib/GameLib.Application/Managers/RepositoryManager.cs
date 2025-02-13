﻿#nullable disable

using Microsoft.Extensions.DependencyInjection;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Shared.Domain.Abstractions.Repository;

namespace GameLib.Application.Managers;

public class RepositoryManager 
{
    private static IServiceScopeFactory _serviceScopeFactory;

    public RepositoryManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    internal static PrizeGroupRepositoryProxy PrizeGroupRepository(Type type)
    {
        var genericType = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService(typeof(IPrizeGroupRepository<>).MakeGenericType(type));
        return new PrizeGroupRepositoryProxy(genericType);
    }

    internal static PrizeRepositoryProxy PrizeRepository(Type type)
    {
        var genericType = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService(typeof(IPrizeRepository<>).MakeGenericType(type));
        return new PrizeRepositoryProxy(genericType);
    }

    internal static IGameConfigurationRepository GameConfigurationRepository()
    {
        return _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IGameConfigurationRepository>();
    }

    internal static IPriceRepository PriceRepository()
    {
        return _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IPriceRepository>();
    }

    internal static ISettingRepository GameSettingRepository()
    {
        return _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISettingRepository>();
    }

    internal static ILimitedPrizeCountsByPlayerRepository LimitedPrizeCountsByPlayerRepository()
    {
        return _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILimitedPrizeCountsByPlayerRepository>();
    }
}

internal class PrizeGroupRepositoryProxy(object repository)
{
    internal IEnumerable<BasePrizeGroup> QueryWithPrizes()
    {
        var queryMethod = repository.GetType().GetMethod(nameof(IPrizeGroupRepository<BasePrizeGroup>.QueryWithTree));

        if (queryMethod != null)
        {
            var prizeGroups = (IEnumerable<BasePrizeGroup>)queryMethod.Invoke(repository, [])!;

            return prizeGroups;
        }

        throw new InvalidCastException();
    }

    internal async Task<BasePrizeGroup> OfIdAsync(int id)
    {
        var ofIdAsyncMethod = repository.GetType().GetMethod(nameof(IPrizeGroupRepository<BasePrizeGroup>.OfIdAsync));

        if (ofIdAsyncMethod != null)
        {
            var task = (dynamic)ofIdAsyncMethod.Invoke(repository, [id])!;
            return (BasePrizeGroup)await task;
        }

        throw new InvalidCastException();
    }

    internal void Update(BasePrizeGroup prizeGroup)
    {
        var updateMethod = repository.GetType().GetMethod(nameof(IPrizeGroupRepository<BasePrizeGroup>.Update));
        var saveAsyncMethod = repository.GetType().GetMethod(nameof(IPrizeGroupRepository<BasePrizeGroup>.SaveAsync));

        if (updateMethod != null && saveAsyncMethod != null)
        {
            _ = (IEnumerable<BasePrizeGroup>)updateMethod.Invoke(repository, [prizeGroup])!;
            _ = saveAsyncMethod.Invoke(repository, []);
        }
    }
}

internal class PrizeRepositoryProxy(object repository)
{
    internal async Task<BasePrize> OfIdAsync(int id)
    {
        var ofIdAsyncMethod = repository.GetType().GetMethod(nameof(IPrizeRepository<BasePrize>.OfIdAsync));

        if (ofIdAsyncMethod != null)
        {
            var task = (dynamic)ofIdAsyncMethod.Invoke(repository, [id])!;
            return (BasePrize)await task;
        }

        throw new InvalidCastException();
    }

    internal void Update(BasePrize prize)
    {
        var updateMethod = repository.GetType().GetMethod(nameof(IPrizeRepository<BasePrize>.Update));
        var saveAsyncMethod = repository.GetType().GetMethod(nameof(IPrizeRepository<BasePrize>.SaveAsync));

        if (updateMethod != null && saveAsyncMethod != null)
        {
            _ = (IEnumerable<BasePrize>)updateMethod.Invoke(repository, [prize])!;
            _ = saveAsyncMethod.Invoke(repository, []);
        }
    }
}