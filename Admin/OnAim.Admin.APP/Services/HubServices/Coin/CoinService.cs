﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;
using System.Collections.Immutable;

namespace OnAim.Admin.APP.Services.Hub.Coin;

public class CoinService : ICoinService
{
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly IReadOnlyRepository<WithdrawOptionGroup> _withdrawOptionGroupRepository;
    private readonly ICoinRepository _coinRepository;

    public CoinService(
        IReadOnlyRepository<WithdrawOption> withdrawOptionRepository,
        IReadOnlyRepository<WithdrawOptionGroup> withdrawOptionGroupRepository,
        ICoinRepository coinRepository
        )
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _coinRepository = coinRepository;
    }

    public async Task<ApplicationResult> GetAllCoins(BaseFilter baseFilter)
    {
        var temps = await _coinRepository.GetCoinTemplates();
        return new ApplicationResult { Data = temps, Success = true };
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        var coin = await _coinRepository.GetCoinTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("Coin Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<ApplicationResult> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate)
    {
        var temp = new CoinTemplate
        {
            Name = coinTemplate.Name,
            CoinType = (Domain.HubEntities.CoinType)coinTemplate.CoinType,
            Description = coinTemplate.Description,
            ImageUrl = coinTemplate.ImageUrl,
        };

        if (coinTemplate.WithdrawOptionIds != null && coinTemplate.WithdrawOptionIds.Any() &&
        (Contracts.CoinType)coinTemplate.CoinType == Contracts.CoinType._1)
        {
            var withdrawOptions = _withdrawOptionRepository.Query
                (wo => coinTemplate.WithdrawOptionIds.Contains(wo.Id))
                .ToList();

            temp.AddWithdrawOptions(withdrawOptions);
        }

        if (coinTemplate.WithdrawOptionGroupIds != null && coinTemplate.WithdrawOptionGroupIds.Any() &&
        (Contracts.CoinType)coinTemplate.CoinType == Contracts.CoinType._1)
        {
            var withdrawOptionGroupss = _withdrawOptionGroupRepository.Query
                (wo => coinTemplate.WithdrawOptionGroupIds.Contains(wo.Id))
                .ToList();

            temp.AddWithdrawOptionGroups(withdrawOptionGroupss);
        }

        await _coinRepository.AddCoinTemplateAsync(temp);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> DeleteCoinTemplate(string CoinTemplateId)
    {
        var coinTemplate = await _coinRepository.GetCoinTemplateByIdAsync(CoinTemplateId);

        if (coinTemplate == null)
        {
            throw new NotFoundException("Coin Template Not Found");
        }

        coinTemplate.Delete();

        await _coinRepository.UpdateCoinTemplateAsync(CoinTemplateId, coinTemplate);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> UpdateCoinTemplate(UpdateCoinTemplateDto update)
    {
        var coinTemplate = await _coinRepository.GetCoinTemplateByIdAsync(update.Id);

        if (coinTemplate == null || coinTemplate.IsDeleted)
        {
            throw new NotFoundException($"Coin template with the specified ID: [{update.Id}] was not found.");
        }

        coinTemplate.Update(update.Name, update.Description, update.ImageUrl, (Domain.HubEntities.CoinType)update.CoinType);

        if (update.WithdrawOptionIds != null && update.WithdrawOptionIds.Any() &&
        (Contracts.CoinType)coinTemplate.CoinType == Contracts.CoinType._1)
        {
            var withdrawOptions = _withdrawOptionRepository.Query(wo => update.WithdrawOptionIds.Any(woId => woId == wo.Id));

            coinTemplate.SetWithdrawOptions(withdrawOptions);
        }

        await _coinRepository.UpdateCoinTemplateAsync(update.Id, coinTemplate);

        return new ApplicationResult { Success = true };
    }
}
