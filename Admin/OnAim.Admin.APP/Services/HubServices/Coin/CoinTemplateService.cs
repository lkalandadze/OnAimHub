﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Infrasturcture.Interfaces;
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

    public async Task<ApplicationResult> GetAllCoinTemplates(BaseFilter filter)
    {
        var temps = await _coinRepository.GetCoinTemplates();
        var totalCount = temps.Count();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;
        var coinTemplates = temps.Select(x => new CoinTemplateListDto
        {
            Id = x.Id,
            Title = x.Name,
            Description = x.Description,
            CoinType = (CoinType)x.CoinType,
            WithdrawOptions = x.WithdrawOptions?.Select(xx => new WithdrawOptionCoinTempDto
            {
                Title = xx.WithdrawOption.Title,
                Description = xx.WithdrawOption.Description,
                ContentType = (EndpointContentType)xx.WithdrawOption.ContentType,
                Endpoint = xx.WithdrawOption.Endpoint,
                EndpointContent = xx.WithdrawOption.EndpointContent,
                Id = xx.WithdrawOption.Id,
                ImageUrl = xx.WithdrawOption.ImageUrl,
            }).ToList() ?? new List<WithdrawOptionCoinTempDto>(),
            WithdrawOptionGroups = x.WithdrawOptionGroups?.Select(z => new WithdrawOptionGroupCoinTempDto
            {
                Title = z.WithdrawOptionGroup.Title,
                Description= z.WithdrawOptionGroup.Description,
                Id = z.WithdrawOptionGroup.Id,
                ImageUrl = z.WithdrawOptionGroup.ImageUrl,
                PriorityIndex = z.WithdrawOptionGroup.PriorityIndex,
            }).ToList() ?? new List<WithdrawOptionGroupCoinTempDto>(),
        });    

        var res = coinTemplates
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<CoinTemplateListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res.ToList(),
            },
        };
    }

    public async Task<ApplicationResult> GetCoinTemplateById(string id)
    {
        var coin = await _coinRepository.GetCoinTemplateByIdAsync(id);

        var coinTemplate = new CoinTemplateListDto
        {
            Id = coin.Id,
            Title = coin.Name,
            Description = coin.Description,
            CoinType = (CoinType)coin.CoinType,
            WithdrawOptions = coin.WithdrawOptions.Select(xx => new WithdrawOptionCoinTempDto
            {
                Title = xx.WithdrawOption.Title,
                Description = xx.WithdrawOption.Description,
                ContentType = (EndpointContentType)xx.WithdrawOption.ContentType,
                Endpoint = xx.WithdrawOption.Endpoint,
                EndpointContent = xx.WithdrawOption.EndpointContent,
                Id = xx.WithdrawOption.Id,
                ImageUrl = xx.WithdrawOption.ImageUrl,
            }).ToList(),
            WithdrawOptionGroups = coin.WithdrawOptionGroups.Select(z => new WithdrawOptionGroupCoinTempDto
            {
                Title = z.WithdrawOptionGroup.Title,
                Description = z.WithdrawOptionGroup.Description,
                Id = z.WithdrawOptionGroup.Id,
                ImageUrl = z.WithdrawOptionGroup.ImageUrl,
                PriorityIndex = z.WithdrawOptionGroup.PriorityIndex,
            }).ToList(),
        };

        if (coinTemplate == null) throw new NotFoundException("Coin template Not Found");

        return new ApplicationResult { Success = true, Data = coinTemplate };
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
                //.Include(x => x.WithdrawOptionEndpoint)
                //.Include(x => x.WithdrawOptionGroups)
                .ToList();

            var template = withdrawOptions.Select(x => new CoinTemplateWithdrawOption
            {
                Id = x.Id,
                CoinTemplateId = temp.Id,
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
                //.Include(x => x.WithdrawOptions)
                //.Include(x => x.OutCoins)
                .ToList();

            var groupTemplate = withdrawOptionGroupss.Select(x => new CoinTemplateWithdrawOptionGroup
            {
                Id = x.Id,
                CoinTemplateId = temp.Id,
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
public class CoinTemplateListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CoinType CoinType { get; set; }
    public List<WithdrawOptionCoinTempDto> WithdrawOptions { get; set; }
    public List<WithdrawOptionGroupCoinTempDto> WithdrawOptionGroups { get; set; }
}
public class WithdrawOptionCoinTempDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

}
public class WithdrawOptionGroupCoinTempDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int? PriorityIndex { get; set; }
}