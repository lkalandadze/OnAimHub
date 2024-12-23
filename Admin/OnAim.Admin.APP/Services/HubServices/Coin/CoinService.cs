using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.Hub.ClientServices;
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
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly IReadOnlyRepository<WithdrawOptionGroup> _withdrawOptionGroupRepository;
    private readonly IReadOnlyRepository<WithdrawOptionEndpoint> _withdrawOptionEndpointRepository;
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public CoinService(
        IReadOnlyRepository<WithdrawOption> withdrawOptionRepository,
        IReadOnlyRepository<WithdrawOptionGroup> withdrawOptionGroupRepository,
        IReadOnlyRepository<WithdrawOptionEndpoint> WithdrawOptionEndpointRepository,
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options
        )
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _withdrawOptionEndpointRepository = WithdrawOptionEndpointRepository;
        _hubApiClient = hubApiClient;
        _options = options.Value;
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
        var data = _withdrawOptionRepository.Query().Include(x => x.WithdrawOptionEndpoint).Include(x => x.OutCoins);

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
               EndpointContent = x.EndpointContent,
               WithdrawOptionEndpointId = x.WithdrawOptionEndpointId,
               ImageUrl = x.ImageUrl,
               OutCoins = x.OutCoins.Select(o => new OutCoinDto 
               {
                   Id = o.Id,
                   Name = o.Name,
                   Description = o.Description,
                   ImageUrl = o.ImageUrl,
                   PromotionId = o.PromotionId,
                   TemplateId = o.FromTemplateId
               }).ToList(),
               WithdrawOptionGroups = x.WithdrawOptionGroups.Select(x => new WithdrawOptionGroupDto
               {
                   Id = x.Id,
                   Title = x.Title,
                   Description = x.Description,
                   ImageUrl = x.ImageUrl,
                   PriorityIndex = x.PriorityIndex,
                   //OutCoins = x.OutCoins.Select(x => new OutCoinDto
                   //{
                   //    Id = x.Id,
                   //    Name = x.Name,
                   //    Description = x.Description,
                   //    ImageUrl = x.ImageUrl,
                   //    PromotionId = x.PromotionId,
                   //    TemplateId = x.FromTemplateId
                   //}).ToList(),
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
        var data = await _withdrawOptionRepository.Query().Include(x => x.WithdrawOptionGroups).Include(x => x.OutCoins).Include(x => x.WithdrawOptionEndpoint).FirstOrDefaultAsync(x => x.Id == id);

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
        var data = _withdrawOptionGroupRepository.Query().Include(x => x.OutCoins).Include(x => x.WithdrawOptions);

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
                   Id = xx.Id,
                   Name = xx.Name,
                   Description = xx.Description,
                   ImageUrl = xx.ImageUrl,
               }).ToList(),
               WithdrawOptions = x.WithdrawOptions.Select(x => new WithdrawOptionDto
               {
                   Id = x.Id,
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
        var data = await _withdrawOptionGroupRepository.Query().Include(x => x.WithdrawOptions).Include(x => x.OutCoins).FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException("withdraw Option Group not found");

        var item = new WithdrawOptionGroupDto
        {
            Id = data.Id,
            Title = data.Title,
            Description = data.Description,
            ImageUrl = data.ImageUrl,
            PriorityIndex = data.PriorityIndex,
            OutCoins = data.OutCoins.Select(xx => new OutCoinDto
            {
                Id = xx.Id,
                Name = xx.Name,
                Description = xx.Description,
                ImageUrl = xx.ImageUrl,
            }).ToList(),
            WithdrawOptions = data.WithdrawOptions.Select(x => new WithdrawOptionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
            }).ToList()
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

    public async Task<ApplicationResult> CreateWithdrawOption(CreateWithdrawOptionDto option)
    {
        try
        {
            var res = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/CreateWithdrawOption", option);

            return new ApplicationResult { Data = res.StatusCode, Success = true };
        }
        catch (Exception e)
        {

            throw new Exception($"failed to save withdraw option : {e.Message}");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOption(UpdateWithdrawOptionDto option)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/UpdateWithdrawOption", option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option");
        }
    }

    public async Task<ApplicationResult> DeleteWithdrawOption(int id)
    {
        var body = new 
        {
            Id = id
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/DeleteWithdrawOption", body);

            return new ApplicationResult { Data = "Successfully deleted withdraw option", Success = true };
        }
        catch (Exception e)
        {

            throw new Exception($"failed to delete withdraw option : {e.Message}");
        }
    }

    public async Task<ApplicationResult> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpointDto option)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/CreateWithdrawOptionEndpoint", option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option endpoint");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpointDto option)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/UpdateWithdrawOptionEndpoint", option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option endpoint");
        }
    }

    public async Task<ApplicationResult> DeleteWithdrawOptionEndpoint(int id)
    {
        var body = new
        {
            Id = id
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/DeleteWithdrawOptionEndpoint", body);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to delete withdraw option");
        }
    }

    public async Task<ApplicationResult> CreateWithdrawOptionGroup(CreateWithdrawOptionGroupDto option)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/CreateWithdrawOptionGroup", option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option Group");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroupDto option)
    {
        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/UpdateWithdrawOptionGroup", option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option Group");
        }
    }

    public async Task<ApplicationResult> DeleteWithdrawOptiongroup(int id)
    {
        var body = new
        {
            Id = id
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/DeleteWithdrawOptionGroup", body);

            return new ApplicationResult { Success = true };
        }
        catch (Exception e)
        {

            throw new Exception($"failed to delete withdraw option Group: {e.Message}");
        }
    }

    public async Task<ApplicationResult> CreateReward(PlayerPrizeDto dto)
    {
        var body = new
        {
            IsClaimableByPlayer = dto.IsClaimableByPlayer,
            PlayerId = dto.PlayerId,
            SourceId = dto.SourceId,
            ExpirationDate = dto.ExpirationDate,
            Prizes = dto.Prizes
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/CreateReward", body);

            return new ApplicationResult { Success = true };
        }
        catch (Exception e)
        {

            throw new Exception($"failed to create: {e.Message}");
        }
    }

    public async Task<ApplicationResult> DeleteReward(int id)
    {
        var body = new
        {
            Id = id
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/DeleteReward", body);

            return new ApplicationResult { Success = true };
        }
        catch (Exception e)
        {

            throw new Exception($"failed to delete reward: {e.Message}");
        }
    }
}
public class CreateWithdrawOptionDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public int EndpointContentType { get; set; }
    public string EndpointContent { get; set; }
    public int WithdrawOptionEndpointId { get; set; }
    public List<int> WithdrawOptionGroupIds { get; set; } = new List<int>();
}
public class UpdateWithdrawOptionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public int EndpointContentType { get; set; }
    public string EndpointContent { get; set; }
    public int WithdrawOptionEndpointId { get; set; }
    public List<int> WithdrawOptionGroupIds { get; set; } = new List<int>();
}
public class CreateWithdrawOptionEndpointDto
{
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public string Content { get; set; }
    public int ContentType { get; set; }
}
public class UpdateWithdrawOptionEndpointDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public string Content { get; set; }
    public int ContentType { get; set; }
}
public class CreateWithdrawOptionGroupDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int PriorityIndex { get; set; }
    public List<int> WithdrawOptionIds { get; set; } = new List<int>();
}
public class UpdateWithdrawOptionGroupDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int PriorityIndex { get; set; }
    public List<int> WithdrawOptionIds { get; set; } = new List<int>();
}
public class PrizeDto
{
    public decimal Amount { get; set; }
    public string PrizeTypeId { get; set; }
}
public class PlayerPrizeDto
{
    public bool IsClaimableByPlayer { get; set; }
    public int PlayerId { get; set; }
    public int SourceId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<PrizeDto> Prizes { get; set; }
}