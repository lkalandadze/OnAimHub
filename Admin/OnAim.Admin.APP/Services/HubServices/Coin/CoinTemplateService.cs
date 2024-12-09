using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.HubServices.Coin;
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

public class CoinTemplateService : ICoinTemplateService
{
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly IReadOnlyRepository<WithdrawOptionGroup> _withdrawOptionGroupRepository;
    private readonly ICoinRepository _coinRepository;

    public CoinTemplateService(
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

    public async Task<CoinTemplate> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate)
    {
        var temp = new CoinTemplate
        {
            Name = coinTemplate.Name,
            CoinType = (Domain.HubEntities.Enum.CoinType)coinTemplate.CoinType,
            Description = coinTemplate.Description,
            ImageUrl = coinTemplate.ImageUrl,
        };

        if (coinTemplate.WithdrawOptionIds != null && coinTemplate.WithdrawOptionIds.Any() &&
        (Infrasturcture.CoinType)coinTemplate.CoinType == Infrasturcture.CoinType._2)
        {
            var withdrawOptions = _withdrawOptionRepository.Query
                (wo => coinTemplate.WithdrawOptionIds.Contains(wo.Id))
                .ToList();

            var template = withdrawOptions.Select(x => new CoinTemplateWithdrawOption
            {
                WithdrawOption = x,
                WithdrawOptionId = x.Id,
            }).ToList();

            temp.AddWithdrawOptions(template);
        }

        if (coinTemplate.WithdrawOptionGroupIds != null && coinTemplate.WithdrawOptionGroupIds.Any() &&
        (Infrasturcture.CoinType)coinTemplate.CoinType == Infrasturcture.CoinType._2)
        {
            var withdrawOptionGroupss = _withdrawOptionGroupRepository.Query
                (wo => coinTemplate.WithdrawOptionGroupIds.Contains(wo.Id))
                .ToList();

            var groupTemplate = withdrawOptionGroupss.Select(x => new CoinTemplateWithdrawOptionGroup
            {
                WithdrawOptionGroup = x,
                WithdrawOptionGroupId = x.Id,
            }).ToList();

            temp.AddWithdrawOptionGroups(groupTemplate);
        }

        await _coinRepository.AddCoinTemplateAsync(temp);

        return temp;
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

        coinTemplate.Update(update.Name, update.Description, update.ImageUrl, (Domain.HubEntities.Enum.CoinType)update.CoinType);

        if (update.WithdrawOptionIds != null && update.WithdrawOptionIds.Any() &&
        (Infrasturcture.CoinType)coinTemplate.CoinType == Infrasturcture.CoinType._1)
        {
            var withdrawOptions = _withdrawOptionRepository.Query(wo => update.WithdrawOptionIds.Any(woId => woId == wo.Id));

            var upd = withdrawOptions.Select(x => new CoinTemplateWithdrawOption
            {
                WithdrawOption = x,
                WithdrawOptionId = x.Id,
            }).ToList();

            coinTemplate.UpdateWithdrawOptions(upd);
        }

        if (update.WithdrawOptionGroupIds != null && update.WithdrawOptionGroupIds.Any() &&
        (Infrasturcture.CoinType)coinTemplate.CoinType == Infrasturcture.CoinType._1)
        {
            var withdrawOptionGroups = _withdrawOptionGroupRepository.Query(wo => update.WithdrawOptionGroupIds.Any(woId => woId == wo.Id));

            var upd = withdrawOptionGroups.Select(x => new CoinTemplateWithdrawOptionGroup
            {
                WithdrawOptionGroup = x,
                WithdrawOptionGroupId = x.Id,
            }).ToList();

            coinTemplate.UpdateWithdrawOptionGroups(upd);
        }

        await _coinRepository.UpdateCoinTemplateAsync(update.Id, coinTemplate);

        return new ApplicationResult { Success = true };
    }
}
