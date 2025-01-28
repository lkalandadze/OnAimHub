using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.Hub.ClientServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Withdraw;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Infrasturcture.Interfaces;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public class CoinService : BaseService, ICoinService
{
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly IReadOnlyRepository<WithdrawOptionGroup> _withdrawOptionGroupRepository;
    private readonly IReadOnlyRepository<WithdrawOptionEndpoint> _withdrawOptionEndpointRepository;
    private readonly IHubApiClient _hubApiClient;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly HubApiClientOptions _options;

    public CoinService(
        IReadOnlyRepository<WithdrawOption> withdrawOptionRepository,
        IReadOnlyRepository<WithdrawOptionGroup> withdrawOptionGroupRepository,
        IReadOnlyRepository<WithdrawOptionEndpoint> WithdrawOptionEndpointRepository,
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options,
        ISecurityContextAccessor SecurityContextAccessor
        )
    {
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
        _withdrawOptionEndpointRepository = WithdrawOptionEndpointRepository;
        _hubApiClient = hubApiClient;
        _securityContextAccessor = SecurityContextAccessor;
        _options = options.Value;
    }

    #region WO
    public async Task<ApplicationResult<object>> CreateWithdrawOption(CreateWithdrawOptionDto option)
    {
        try
        {
            var body = new
            {
                Title = option.Title,
                Description = option.Description,
                ImageUrl = option.ImageUrl,
                Value = option.Value,
                Endpoint = option.Endpoint,
                EndpointContentType = option.EndpointContentType,
                EndpointContent = option.EndpointContent,
                WithdrawOptionEndpointId = option.WithdrawOptionEndpointId,
                WithdrawOptionGroupIds = option.WithdrawOptionGroupIds,
                CreatedByUserId = _securityContextAccessor.UserId
            };

            var res = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/CreateWithdrawOption", body);

            return new ApplicationResult<object> { Data = res.StatusCode, Success = true };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"{ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> UpdateWithdrawOption(UpdateWithdrawOptionDto option)
    {
        try
        {
            var res = await _hubApiClient.PutAsJson($"{_options.Endpoint}Admin/UpdateWithdrawOption", option);

            return new ApplicationResult<object> { Success = true, Data = res.StatusCode };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to update withdraw option  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> DeleteWithdrawOption(List<int> ids)
    {
        var body = new
        {
            Ids = ids
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/DeleteWithdrawOption", body);

            return new ApplicationResult<object> { Data = "Successfully deleted withdraw option", Success = true };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to delete withdraw option  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<PaginatedResult<WithdrawOptionDto>>> GetAllWithdrawOptions(BaseFilter filter)
    {
        var data = _withdrawOptionRepository.Query().Include(x => x.WithdrawOptionEndpoint).Include(x => x.OutCoins);

        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    data = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<WithdrawOption, ICollection<Domain.HubEntities.Coin.OutCoin>>)data.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    data = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<WithdrawOption, ICollection<Domain.HubEntities.Coin.OutCoin>>)data.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

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
               Value = x.Value,
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

        return new ApplicationResult<PaginatedResult<WithdrawOptionDto>>
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

    public async Task<ApplicationResult<WithdrawOptionDto>> GetWithdrawOptionById(int id)
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

        return new ApplicationResult<WithdrawOptionDto> { Data = item, Success = true };
    }

    #endregion

    #region WOG
    public async Task<ApplicationResult<object>> CreateWithdrawOptionGroup(CreateWithdrawOptionGroupDto option)
    {
        try
        {
            var body = new
            {
                Title = option.Title,
                Description = option.Description,
                ImageUrl = option.ImageUrl,
                PriorityIndex = option.PriorityIndex,
                WithdrawOptionIds = option.WithdrawOptionIds,
                CreatedByUserId = _securityContextAccessor.UserId,
            };

            var res = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/CreateWithdrawOptionGroup", option);

            return new ApplicationResult<object> { Success = true, Data = res.StatusCode };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to save withdraw option Group  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroupDto option)
    {
        try
        {
            var res = await _hubApiClient.PutAsJson($"{_options.Endpoint}Admin/UpdateWithdrawOptionGroup", option);

            return new ApplicationResult<object> { Success = true, Data = res.StatusCode };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to update withdraw option Group  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> DeleteWithdrawOptiongroup(List<int> ids)
    {
        var body = new
        {
            Ids = ids
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/DeleteWithdrawOptionGroup", body);

            return new ApplicationResult<object> { Success = true, Data = "Deleted Successfully" };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to delete withdraw option Group  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<PaginatedResult<WithdrawOptionGroupDto>>> GetAllWithdrawOptionGroups(BaseFilter filter)
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

        return new ApplicationResult<PaginatedResult<WithdrawOptionGroupDto>>
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

    public async Task<ApplicationResult<WithdrawOptionGroupDto>> GetWithdrawOptionGroupById(int id)
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

        return new ApplicationResult<WithdrawOptionGroupDto> { Data = item, Success = true };
    }

    #endregion

    #region WOE

    public async Task<ApplicationResult<object>> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpointDto option)
    {
        try
        {
            var body = new
            {
                Name = option.Name,
                Endpoint = option.Endpoint,
                Content = option.Content,
                CreatedByUserId = _securityContextAccessor.UserId,
                ContentType = option.ContentType
            };


            var res = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/CreateWithdrawOptionEndpoint", body);

            return new ApplicationResult<object> { Success = true, Data = res.StatusCode };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to save withdraw option endpoint  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpointDto option)
    {
        try
        {
            var res = await _hubApiClient.PutAsJson($"{_options.Endpoint}Admin/UpdateWithdrawOptionEndpoint", option);

            return new ApplicationResult<object> { Success = true, Data = res.StatusCode };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to update withdraw option endpoint  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> DeleteWithdrawOptionEndpoint(List<int> ids)
    {
        var body = new
        {
            Ids = ids
        };

        try
        {
            await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/DeleteWithdrawOptionEndpoint", body);

            return new ApplicationResult<object> { Success = true, Data = "Deleted Successfully" };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"failed to delete withdraw option  {ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<PaginatedResult<WithdrawOptionEndpointDto>>> GetWithdrawOptionEndpoints(BaseFilter filter)
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

        return new ApplicationResult<PaginatedResult<WithdrawOptionEndpointDto>>
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

    public async Task<ApplicationResult<WithdrawOptionEndpointDto>> GetWithdrawOptionEndpointById(int id)
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

        return new ApplicationResult<WithdrawOptionEndpointDto> { Data = item, Success = true };
    }

    #endregion

    public async Task<ApplicationResult<object>> CreateReward(PlayerPrizeDto dto)
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
            var res = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/CreateReward", dto);

            return new ApplicationResult<object> { Success = true, Data = res.StatusCode };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"{ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
    }

    public async Task<ApplicationResult<object>> DeleteReward(int id)
    {
        var body = new
        {
            Id = id
        };

        try
        {
            await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/DeleteReward", body);

            return new ApplicationResult<object> { Success = true, Data = "Deleted Successfully" };
        }
        catch (Exception ex)
        {
            return await Fail(new Error
            {
                Message = $"{ex.Message}",
                Code = StatusCodes.Status500InternalServerError,
            });
        }
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
}