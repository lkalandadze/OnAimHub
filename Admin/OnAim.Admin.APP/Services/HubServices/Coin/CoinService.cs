using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Withdraw;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Infrasturcture.Interfaces;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public class CoinService : ICoinService
{
    private readonly HubClientService _hubClientService;
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly IReadOnlyRepository<WithdrawOptionGroup> _withdrawOptionGroupRepository;
    private readonly IReadOnlyRepository<WithdrawOptionEndpoint> _withdrawOptionEndpointRepository;

    public CoinService(
        HubClientService hubClientService,
        IReadOnlyRepository<WithdrawOption> withdrawOptionRepository,
        IReadOnlyRepository<WithdrawOptionGroup> withdrawOptionGroupRepository,
        IReadOnlyRepository<WithdrawOptionEndpoint> WithdrawOptionEndpointRepository)
    {
        _hubClientService = hubClientService;
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _withdrawOptionEndpointRepository = WithdrawOptionEndpointRepository;
    }

    public async Task<IEnumerable<WithdrawOption>> GetWithdrawOptions(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel)
    {
        if (outCoinModel == null)
        {
            throw new Exception(
                "The provided model is null. Please ensure that a valid OutCoin model is supplied."
            );
        }

        var withdrawOptions = _withdrawOptionRepository.Query(wo => outCoinModel.WithdrawOptionIds.Any(woId => woId == wo.Id));

        if (withdrawOptions == null || !withdrawOptions.Any())
        {
            throw new Exception(
                "No withdraw options were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing segments."
            );
        }

        return withdrawOptions;
    }

    public async Task<IEnumerable<WithdrawOptionGroup>> GetWithdrawOptionGroups(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel)
    {
        if (outCoinModel == null)
        {
            throw new Exception(
                "The provided model is null. Please ensure that a valid OutCoin model is supplied."
            );
        }

        var withdrawOptionGroups = _withdrawOptionGroupRepository.Query(wog => outCoinModel.WithdrawOptionGroupIds.Any(wogId => wogId == wog.Id));

        if (withdrawOptionGroups == null || !withdrawOptionGroups.Any())
        {
            throw new Exception(
                "No withdraw option groups were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing segments."
            );
        }

        return withdrawOptionGroups;
    }

    public async Task<ApplicationResult> GetAllWithdrawOptions(BaseFilter filter)
    {
        var data = _withdrawOptionRepository.Query();

        var totalCount = await data.CountAsync();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = data
           .Select(x => new WithdrawOptionDto
           {
               Id = x.Id,
               Title = x.Title,
               Description = x.Description,
               ContentType = (Contracts.Dtos.Withdraw.EndpointContentType)x.ContentType,
               Endpoint = x.Endpoint,
               //EndpointContent = x.EndpointContent,
               ImageUrl = x.ImageUrl,
               WithdrawOptionGroups = x.WithdrawOptionGroups.Select(x => new WithdrawOptionGroupDto
               {
                   Title = x.Title,
                   Description = x.Description,
                   ImageUrl = x.ImageUrl,
                   PriorityIndex = x.PriorityIndex,
                   OutCoins = x.OutCoins.Select(x => new OutCoinDto
                   {
                       Name = x.Name,
                       Description = x.Description,
                       ImageUrl = x.ImageUrl,
                   }).ToList(),
               }).ToList(),
           })
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<WithdrawOptionDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
            },
        };
    }

    public async Task<ApplicationResult> GetWithdrawOptionById(int id)
    {
        var data = await _withdrawOptionRepository.Query().Include(x => x.OutCoins).FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException("withdraw Option not found");

        var item = new WithdrawOptionDto
        {
            Id = data.Id,
            Title = data.Title,
            ContentType = (Contracts.Dtos.Withdraw.EndpointContentType)data.ContentType,
            Description = data.Description,
            Endpoint = data.Endpoint,
            EndpointContent = data.EndpointContent,
            ImageUrl = data.ImageUrl,
            OutCoins = data.OutCoins?.Select(x => new OutCoinDto
            {
                ImageUrl = x.ImageUrl,
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                PromotionId = x.PromotionId,
                TemplateId = x.FromTemplateId
            }).ToList() ?? new List<OutCoinDto>(),
            WithdrawOptionEndpointId = data.WithdrawOptionEndpointId,
            WithdrawOptionGroups = data.WithdrawOptionGroups?.Select(z => new WithdrawOptionGroupDto
            {
                Id = z.Id,
                Title = z.Title,
                Description = z.Description,
                ImageUrl = z.ImageUrl,
                PriorityIndex = z.PriorityIndex,
            }).ToList() ?? new List<WithdrawOptionGroupDto>(),
        };

        return new ApplicationResult { Data = item, Success = true };
    }

    public async Task<ApplicationResult> GetAllWithdrawOptionGroups(BaseFilter filter)
    {
        var data = _withdrawOptionGroupRepository.Query();

        var totalCount = await data.CountAsync();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = data
           .Select(x => new WithdrawOptionGroupDto
           {
               Id = x.Id,
               Title = x.Title,
               Description = x.Description,
               ImageUrl = x.ImageUrl,
               PriorityIndex = x.PriorityIndex,
               OutCoins = x.OutCoins.Select(xx => new OutCoinDto
               {
                   Name = xx.Name,
                   Description = xx.Description,
                   ImageUrl = xx.ImageUrl,
               }).ToList(),
               WithdrawOptions = x.WithdrawOptions.Select(x => new WithdrawOptionDto
               {
                   Title = x.Title,
                   Description = x.Description,
                   ImageUrl = x.ImageUrl,
               }).ToList()
           })
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<WithdrawOptionGroupDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
            },
        };
    }

    public async Task<ApplicationResult> GetWithdrawOptionGroupById(int id)
    {
        var data = await _withdrawOptionGroupRepository.Query().Include(x => x.OutCoins).FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException("withdraw Option Group not found");

        var item = new WithdrawOptionGroupDto
        {
            Id = data.Id,
            Description = data.Description,
            ImageUrl = data.ImageUrl,
            OutCoins = data.OutCoins?.Select(x => new OutCoinDto
            {
                ImageUrl = x.ImageUrl,
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                PromotionId = x.PromotionId,
                TemplateId = x.FromTemplateId
            }).ToList() ?? new List<OutCoinDto>(),
            PriorityIndex = data.PriorityIndex,
            Title = data.Title,
            WithdrawOptions = data.WithdrawOptions?.Select(x => new WithdrawOptionDto
            {
                Id = x.Id,
                Title = x.Title,
                ContentType = (Contracts.Dtos.Withdraw.EndpointContentType)x.ContentType,
                Description = x.Description,
                Endpoint = x.Endpoint,
                EndpointContent = x.EndpointContent,
                ImageUrl = x.ImageUrl,
                WithdrawOptionEndpointId = x.WithdrawOptionEndpointId,
            }).ToList() ?? new List<WithdrawOptionDto>()
        };

        return new ApplicationResult { Data = item, Success = true };
    }

    public async Task<ApplicationResult> GetWithdrawOptionEndpoints(BaseFilter filter)
    {
        var data = _withdrawOptionEndpointRepository.Query();

        var totalCount = await data.CountAsync();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = data
           .Select(x => new WithdrawOptionEndpointDto
           {
               Id = x.Id,
               Name = x.Name,
               Endpoint = x.Endpoint,
               Content = x.Content,
               ContentType = (Contracts.Dtos.Withdraw.EndpointContentType)x.ContentType
           })
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<WithdrawOptionEndpointDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
            },
        };
    }

    public async Task<ApplicationResult> GetWithdrawOptionEndpointById(int id)
    {
        var data = await _withdrawOptionEndpointRepository.Query().FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException("withdraw Option Endpoint not found");

        var item = new WithdrawOptionEndpointDto
        {
            Id = data.Id,
            Content = data.Content,
            ContentType = (Contracts.Dtos.Withdraw.EndpointContentType)data.ContentType,
            Endpoint = data.Endpoint,
            Name = data.Name,
        };

        return new ApplicationResult { Data = item, Success = true };
    }

    public async Task<ApplicationResult> CreateWithdrawOption(CreateWithdrawOption option)
    {
        try
        {
            await _hubClientService.CreateWithdrawOptionAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOption(UpdateWithdrawOption option)
    {
        try
        {
            await _hubClientService.UpdateWithdrawOptionAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option");
        }
    }

    public async Task<ApplicationResult> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpoint option)
    {
        try
        {
            await _hubClientService.CreateWithdrawOptionEndpointAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option endpoint");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpoint option)
    {
        try
        {
            await _hubClientService.UpdateWithdrawOptionEndpointAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option endpoint");
        }
    }

    public async Task<ApplicationResult> CreateWithdrawOptionGroup(CreateWithdrawOptionGroup option)
    {
        try
        {
            await _hubClientService.CreateWithdrawOptionGroupAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option Group");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroup option)
    {
        try
        {
            await _hubClientService.UpdateWithdrawOptionGroupAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option Group");
        }
    }
}