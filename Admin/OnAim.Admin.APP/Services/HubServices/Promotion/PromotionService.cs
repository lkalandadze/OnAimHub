using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.APP.Services.Hub.Promotion;

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository<Domain.HubEntities.Promotion> _promotionRepository;
    private readonly IPromotionRepository<Domain.HubEntities.PromotionCoin> _coinRepo;
    private readonly HubClientService _hubClientService;
    private readonly SagaClient _sagaClient;

    public PromotionService(
        IPromotionRepository<Domain.HubEntities.Promotion> promotionRepository,
        IPromotionRepository<Domain.HubEntities.PromotionCoin> coinRepo,
        HubClientService hubClientService,
        SagaClient sagaClient
        )
    {
        _promotionRepository = promotionRepository;
        _coinRepo = coinRepo;
        _hubClientService = hubClientService;
        _sagaClient = sagaClient;
    }

    public async Task<ApplicationResult> GetAllPromotions(PromotionFilter filter)
    {
        var promotions = _promotionRepository.Query(x =>
                         string.IsNullOrEmpty(filter.Name) || EF.Functions.Like(x.Title, $"{filter.Name}%"))
             //.Include(x => x.Coins)
             //.ThenInclude(x => x.WithdrawOptions)
             .AsNoTracking();

        var promList = promotions.ToList();

        if (filter.Status.HasValue)
            promotions = promotions.Where(x => x.Status == (Domain.HubEntities.PromotionStatus)filter.Status.Value);

        if (filter.StartDate.HasValue)
            promotions = promotions.Where(x => x.StartDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            promotions = promotions.Where(x => x.EndDate <= filter.EndDate.Value);

        var totalCount = await promotions.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;
        bool sortDescending = filter.SortDescending.GetValueOrDefault();

        if (filter.SortBy == "Id" || filter.SortBy == "id")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.Id)
                : promotions.OrderBy(x => x.Id);
        }
        else if (filter.SortBy == "name" || filter.SortBy == "Name")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.Title)
                : promotions.OrderBy(x => x.Title);
        }
        else if (filter.SortBy == "status" || filter.SortBy == "Status")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.Status)
                : promotions.OrderBy(x => x.Status);
        }
        else if (filter.SortBy == "startDate" || filter.SortBy == "StartDate")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.StartDate)
                : promotions.OrderBy(x => x.StartDate);
        }
        else if (filter.SortBy == "endDate" || filter.SortBy == "EndDate")
        {
            promotions = sortDescending
                ? promotions.OrderByDescending(x => x.EndDate)
                : promotions.OrderBy(x => x.EndDate);
        }

        var res = promotions
            .Select(x => new PromotionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                TotalCost = x.TotalCost,
                Status = (Contracts.Dtos.Promotion.PromotionStatus)x.Status,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PromotionCoins = x.Coins.Select(xx => new PromotionCoinDto
                {
                    Id = xx.Id,
                    PromotionId = xx.PromotionId,
                    Name = xx.Name,
                    Description = xx.Description,
                    ImageUrl = xx.ImageUrl,
                    CoinType = (Contracts.Dtos.Promotion.CoinType)xx.CoinType,
                    WithdrawOptions = xx.WithdrawOptions.Select(xxx => new WithdrawOptionDto
                    {
                        Title = xxx.Title,
                        Description = xxx.Description,
                        ImageUrl = xxx.ImageUrl,
                    }).ToList()
                }).ToList()
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);


        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<PromotionDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
                SortableFields = new List<string> { "Id", "Name", "Status" },
            },
        };
    }

    public async Task<ApplicationResult> GetPromotionById(int id)
    {
        var promotion = await _promotionRepository.Query().Include(x => x.Coins).ThenInclude(x => x.WithdrawOptions).FirstOrDefaultAsync(x => x.Id == id);

        if (promotion == null) throw new NotFoundException("promotion not found");

        var result = new PromotionDto
        {
            Id = promotion.Id,
            Title = promotion.Title,
            Description = promotion.Description,
            StartDate = promotion.StartDate,
            EndDate = promotion.EndDate,
            PromotionCoins = promotion.Coins.Select(x => new PromotionCoinDto
            {
                Description = x.Description,
                Id = x.Id,
                PromotionId = x.PromotionId,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                CoinType = (Contracts.Dtos.Promotion.CoinType)x.CoinType,
                WithdrawOptions = x.WithdrawOptions.Select(xxx => new WithdrawOptionDto
                {
                    Title = xxx.Title,
                    Description = xxx.Description,
                    ImageUrl = xxx.ImageUrl,
                }).ToList()
            }).ToList(),

            Segments = (List<Contracts.Dtos.Segment.SegmentDto>)promotion.Segments,
        };


        return new ApplicationResult { Success = true, Data = promotion };
    }

    public async Task<ApplicationResult> CreatePromotion(CreatePromotionDto create)
    {
        try
        {
            await _sagaClient.SagaAsync(create);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> CreatePromotionView(CreatePromotionView create)
    {
        try
        {
            await _hubClientService.CreatePromotionViewAsync(create);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> UpdatePromotionStatus(UpdatePromotionStatusCommand update)
    {
        try
        {
            await _hubClientService.UpdatePromotionStatusAsync(update);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> DeletePromotion(SoftDeletePromotionCommand command)
    {
        try
        {
            await _hubClientService.SoftDeletePromotionAsync(command);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}