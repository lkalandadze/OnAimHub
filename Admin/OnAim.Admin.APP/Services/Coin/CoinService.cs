using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Domain.Interfaces;

namespace OnAim.Admin.APP.Services.Coin;

public class CoinService : ICoinService
{
    private readonly IReadOnlyRepository<Admin.Domain.HubEntities.PromotionCoin> _repository;

    public CoinService(IReadOnlyRepository<OnAim.Admin.Domain.HubEntities.PromotionCoin> repository)
    {
        _repository = repository;
    }
    public async Task<ApplicationResult> GetAllCoins(BaseFilter baseFilter)
    {
        //var coins = _coinRepository.Query();

        //var totalCount = await coins.CountAsync();

        //var pageNumber = baseFilter.PageNumber ?? 1;
        //var pageSize = baseFilter.PageSize ?? 25;

        //bool sortDescending = baseFilter.SortDescending.GetValueOrDefault();

        //if (baseFilter.SortBy == "Id" || baseFilter.SortBy == "id")
        //{
        //    coins = sortDescending
        //        ? coins.OrderByDescending(x => x.Id)
        //        : coins.OrderBy(x => x.Id);
        //}
        //else if (baseFilter.SortBy == "Name" || baseFilter.SortBy == "name")
        //{
        //    coins = sortDescending
        //        ? coins.OrderByDescending(x => x.Name)
        //        : coins.OrderBy(x => x.Name);
        //}

        //var result = coins.Skip(pageNumber).Take(pageSize);

        //return new ApplicationResult
        //{
        //    Success = true,
        //    Data = new PaginatedResult<OnAim.Admin.Domain.HubEntities.Coin>{
        //        PageNumber = pageNumber,
        //        PageSize = pageSize,
        //        TotalCount = totalCount,
        //        Items = await result.ToListAsync(),
        //        SortableFields = new List<string>()
        //    }
        //};
        return new ApplicationResult();
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        //var coin = await _coinRepository.Query().FirstOrDefaultAsync(x => x.Id == id);

        //if (coin == null) throw new NotFoundException("Coin Not Found");

        return new ApplicationResult { Success = true, Data = "" };
    }
}
