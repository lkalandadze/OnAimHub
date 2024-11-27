using MongoDB.Bson;
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

namespace OnAim.Admin.APP.Services.Coin;

public class CoinService : ICoinService
{
    private readonly IReadOnlyRepository<Admin.Domain.HubEntities.Coin> _repository;
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly ICoinRepository _coinRepository;

    public CoinService(
        IReadOnlyRepository<Admin.Domain.HubEntities.Coin> repository,
        IReadOnlyRepository<WithdrawOption> withdrawOptionRepository,
        ICoinRepository coinRepository
        )
    {
        _repository = repository;
        _withdrawOptionRepository = withdrawOptionRepository;
        _coinRepository = coinRepository;
    }

    public async Task<ApplicationResult> GetAllCoins(BaseFilter baseFilter)
    {
        var temps = await _coinRepository.GetCoinTemplates();
        return new ApplicationResult { Data = temps, Success = true};
    }

    public async Task<ApplicationResult> GetById(ObjectId id)
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
            CoinType = (Admin.Domain.CoinType)coinTemplate.CoinType,
            Description = coinTemplate.Description,
            ImageUrl = coinTemplate.ImageUrl,
        };

        if (coinTemplate.WithdrawOptionIds != null && coinTemplate.WithdrawOptionIds.Any())
        {
            var withdrawOptions = _withdrawOptionRepository.Query(wo => coinTemplate.WithdrawOptionIds.Contains(wo.Id)).ToList();

            var withdrawOptionAdmins = withdrawOptions.Select(withdrawOption => new WithdrawOptionAdmin
            {
                Id = withdrawOption.Id,
                Title = withdrawOption.Title,
                Description = withdrawOption.Description,
                CoinTemplates = withdrawOption.CoinTemplates,
                ContentType = withdrawOption.ContentType,
                Endpoint = withdrawOption.Endpoint,
                EndpointContent = withdrawOption.EndpointContent,
                ImageUrl = withdrawOption.ImageUrl,
                PromotionCoins = withdrawOption.PromotionCoins,
                WithdrawEndpointTemplate = withdrawOption.WithdrawEndpointTemplate,
                WithdrawEndpointTemplateId = withdrawOption.WithdrawEndpointTemplateId,
                WithdrawOptionGroups = withdrawOption.WithdrawOptionGroups,
            }).ToList();

            temp.AddWithdrawOptions(withdrawOptionAdmins);
        }

        await _coinRepository.AddCoinTemplateAsync(temp);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> DeleteCoinTemplate(ObjectId CoinTemplateId)
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
        ObjectId.TryParse(update.Id, out var objectId);
        var coinTemplate = await _coinRepository.GetCoinTemplateByIdAsync(objectId);

        if (coinTemplate == null || coinTemplate.IsDeleted)
        {
            throw new NotFoundException($"Coin template with the specified ID: [{update.Id}] was not found.");
        }

        coinTemplate.Update(update.Name, update.Description, update.ImageUrl, (Admin.Domain.CoinType)update.CoinType);

        if (update.WithdrawOptionIds != null && update.WithdrawOptionIds.Any())
        {
            var withdrawOptions = (_withdrawOptionRepository.Query(wo => update.WithdrawOptionIds.Any(woId => woId == wo.Id)));
            var withdrawOptionAdmins = withdrawOptions.Select(withdrawOption => new WithdrawOptionAdmin
            {
                Id = withdrawOption.Id,
                Title = withdrawOption.Title,
                Description = withdrawOption.Description,
                CoinTemplates = withdrawOption.CoinTemplates,
                ContentType = withdrawOption.ContentType,
                Endpoint = withdrawOption.Endpoint,
                EndpointContent = withdrawOption.EndpointContent,
                ImageUrl = withdrawOption.ImageUrl,
                PromotionCoins = withdrawOption.PromotionCoins,
                //WithdrawEndpointTemplate = withdrawOption.WithdrawEndpointTemplate,
                //WithdrawEndpointTemplateId = withdrawOption.WithdrawEndpointTemplateId,
                WithdrawOptionGroups = withdrawOption.WithdrawOptionGroups,
            }).ToList();
            coinTemplate.SetWithdrawOptions(withdrawOptionAdmins);
        }

        await _coinRepository.UpdateCoinTemplateAsync(objectId, coinTemplate);

        return new ApplicationResult { Success = true };
    }
}
